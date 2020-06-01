using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Messages;

namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Base actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    public abstract class EventStoreActor : Actor, IEventStoreActor
    {
        private const string _stateName = "EventStore";
        private readonly IActorSystem _actorSystem;
        private long _transactionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStoreActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        /// <param name="actorSystem">The custom implementation of the actor system.</param>
        protected EventStoreActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null, IActorSystem? actorSystem = null) : base(actorService, actorId, actorStateManager)
        {
            _actorSystem = actorSystem ?? new ActorSystem();
        }

        /// <summary>
        /// Gets the transaction identifier.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>System.String.</returns>
        public static string GetTransactionId(string name, long id) => $"{name}-({id})";

        /// <summary>
        /// Appends the events to the store.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns>The new version.</returns>
        public async Task<long> AppendEvents(IList<IEvent> events)
        {
            if (events.Any() == true)
            {
                _transactionId++;
                try
                {
                    await _actorSystem.Create<IEventStoreTransactionActor>(GetTransactionId(_transactionId)).SetEvents(events);
                }
                catch
                {
                    if (await _actorSystem.Create<IEventStoreTransactionActor>(GetTransactionId(_transactionId)).HasEvents())
                    {
                        // The transaction id already exist. Seems that the last transaction id
                        // value is corrupt. Reset the value and retry.
                        await ResetVersion();
                        await _actorSystem.Create<IEventStoreTransactionActor>(GetTransactionId(_transactionId)).SetEvents(events);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            await StateManager.SetStateAsync(_stateName, _transactionId);
            return _transactionId;
        }

        /// <summary>
        /// Gets the lastest version.
        /// </summary>
        /// <returns>The lastest version number of the stream.</returns>
        public Task<long> GetLastestVersion() => Task.FromResult(_transactionId);

        /// <summary>
        /// Resets the version value by reading the event stream store.
        /// </summary>
        /// <remarks>This operation can be very long. Reset only if the version number is corrupted.</remarks>
        /// <returns>The version number of the last session in the store</returns>
        public async Task<long> ResetVersion()
        {
            _transactionId = 0L;
            while (await _actorSystem.Create<IEventStoreTransactionActor>(GetTransactionId(_transactionId + 1)).HasEvents())
            {
                _transactionId++;
            }

            await StateManager.SetStateAsync(_stateName, _transactionId);
            return _transactionId;
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            _transactionId = await StateManager.GetOrAddStateAsync(_stateName, 0L);
        }

        private string GetTransactionId(long id) => GetTransactionId(Id.GetId(), id);
    }
}
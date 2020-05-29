using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Domain;
using ExtenFlow.Messages;

namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Base actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    public abstract class EventStreamActor : Actor, IEventStreamActor
    {
        private const string _stateName = "EventStream";
        private readonly IActorSystem _actorSystem;
        private long _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        /// <param name="actorSystem">The custom implementation of the actor system.</param>
        protected EventStreamActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null, IActorSystem? actorSystem = null) : base(actorService, actorId, actorStateManager)
        {
            _actorSystem = actorSystem ?? new ActorSystem();
        }

        /// <summary>
        /// Appends the events to the store.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns>The new version.</returns>
        public async Task<long> AppendEvents(IList<IEvent> events)
        {
            if (events.Any() == true)
            {
                _version++;
                await _actorSystem.Create<IEventStreamSessionActor>(EventStreamSessionHelper.GetSessionId(Id.GetId(), _version)).SetEvents(events);
            }
            await StateManager.SetStateAsync(_stateName, _version);
            return _version;
        }

        /// <summary>
        /// Gets the lastest version.
        /// </summary>
        /// <returns>The lastest version number of the stream.</returns>
        public Task<long> GetLastestVersion() => Task.FromResult(_version);

        /// <summary>
        /// Resets the version value by reading the event stream store.
        /// </summary>
        /// <remarks>This operation can be very long. Reset only if the version number is corrupted.</remarks>
        /// <returns>The version number of the last session in the store</returns>
        public async Task<long> ResetVersion()
        {
            _version = 1L;
            while (await _actorSystem.Create<IEventStreamSessionActor>(EventStreamSessionHelper.GetSessionId(Id.GetId(), _version)).HasEvents())
            {
                _version++;
            }

            _version--;
            await StateManager.SetStateAsync(_stateName, _version);
            return _version;
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            _version = await StateManager.GetOrAddStateAsync(_stateName, 0L);
        }
    }
}
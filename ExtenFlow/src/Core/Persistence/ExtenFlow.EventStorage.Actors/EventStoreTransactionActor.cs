using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Messages;

namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Class EventStoreTransactionActor. Implements the <see cref="Dapr.Actors.Runtime.Actor"/>
    /// Implements the <see cref="ExtenFlow.EventStorage.Actors.IEventStoreTransactionActor"/>
    /// </summary>
    /// <seealso cref="Dapr.Actors.Runtime.Actor"/>
    /// <seealso cref="ExtenFlow.EventStorage.Actors.IEventStoreTransactionActor"/>
    public class EventStoreTransactionActor : Actor, IEventStoreTransactionActor
    {
        private const string _stateName = "Events";
        private List<IEvent>? _events;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStoreTransactionActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="actorStateManager">The actor state manager.</param>
        public EventStoreTransactionActor(
            ActorService actorService,
            ActorId actorId,
            IActorStateManager? actorStateManager = null)
            : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task&lt;System.Collections.Generic.IEnumerable&lt;ExtenFlow.Messages.IEvent&gt;&gt;.</returns>
        public Task<IEnumerable<IEvent>> GetEvents()
            => Task.FromResult<IEnumerable<IEvent>>(
                _events ??
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Messages.Properties.Resources.EventStoreTransactionNotFound, Id.GetId()))
                );

        /// <summary>
        /// Determines whether this instance has events.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> HasEvents() => Task.FromResult(_events != null);

        /// <summary>
        /// Sets the events.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task SetEvents(IList<IEvent> events)
        {
            if (_events != null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExtenFlow.Messages.Properties.Resources.DuplicateEventStoreTransactionId, Id.GetId()));
            }
            _events = new List<IEvent>(events);
            return StateManager.SetStateAsync(_stateName, _events);
        }

        /// <summary>
        /// on activate as an asynchronous operation.
        /// </summary>
        /// <returns>
        /// A <see cref="Task">Task</see> that represents outstanding OnActivateAsync operation.
        /// </returns>
        protected override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            ConditionalValue<List<IEvent>> result = await StateManager.TryGetStateAsync<List<IEvent>>(_stateName);
            if (result.HasValue)
            {
                _events = result.Value;
            }
            else
            {
                _events = null;
            }
        }
    }
}
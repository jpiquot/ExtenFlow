using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Domain;
using ExtenFlow.Messages;

namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Class EventStreamSessionActor. Implements the <see cref="Dapr.Actors.Runtime.Actor"/>
    /// Implements the <see cref="ExtenFlow.EventStorage.Actors.IEventStreamSessionActor"/>
    /// </summary>
    /// <seealso cref="Dapr.Actors.Runtime.Actor"/>
    /// <seealso cref="ExtenFlow.EventStorage.Actors.IEventStreamSessionActor"/>
    public class EventStreamSessionActor : Actor, IEventStreamSessionActor
    {
        private const string _stateName = "EventStreamSession";
        private HashSet<IEvent>? _events;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamSessionActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="actorStateManager">The actor state manager.</param>
        protected EventStreamSessionActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns>Task&lt;IList&lt;IEvent&gt;&gt;.</returns>
        public Task<IList<IEvent>> GetEvents()
        {
            if (_events?.Any() == true)
            {
                return Task.FromResult((IList<IEvent>)_events);
            }
            return Task.FromException<IList<IEvent>>(new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resources.EventStoreSessionNotFound, // The event store session '%1' not found.
                            Id.GetId()
                            )));
        }

        /// <summary>
        /// Determines whether this instance has events.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> HasEvents() => Task.FromResult(_events?.Any() == true);

        /// <summary>
        /// Sets the events.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns>Task.</returns>
        public Task SetEvents(IList<IEvent> events)
        {
            if (_events?.Any() == true)
            {
                // Duplicate event store session '%1'. The stream version number may be corrupted.
                return Task.FromException(
                    new InvalidOperationException(string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.DuplicateEventStoreSession,
                        Id.GetId()
                    )));
            }
            _events = new HashSet<IEvent>(events);
            return StateManager.SetStateAsync(_stateName, _events);
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            _events = await StateManager.GetOrAddStateAsync(_stateName, new HashSet<IEvent>());
        }
    }
}
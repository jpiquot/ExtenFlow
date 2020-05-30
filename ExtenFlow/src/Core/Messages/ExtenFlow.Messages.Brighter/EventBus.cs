using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExtenFlow.Infrastructure;
using ExtenFlow.Messages;

namespace ExtenFlow.EventBus.InMemory
{
    /// <summary>
    /// Class EventBus. Implements the <see cref="ExtenFlow.EventBus.IEventBus"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.EventBus.IEventBus"/>
    public class EventBus : IEventBus
    {
        private Dictionary<Type, Dictionary<string, Func<IEvent, Task>>> _suscribers = new Dictionary<Type, Dictionary<string, Func<IEvent, Task>>>();

        /// <summary>
        /// Publishes the specified events.
        /// </summary>
        /// <param name="events">The events to publish.</param>
        /// <returns>Task.</returns>
        public Task Publish(IEnumerable<IEvent> events)
            => Task
                .WhenAll(events
                    .Select(p => Publish(p)).ToList());

        /// <summary>
        /// Publishes the specified one event.
        /// </summary>
        /// <param name="oneEvent">The event to publish.</param>
        /// <returns>Task.</returns>
        public Task Publish(IEvent oneEvent)
            => Task
                .WhenAll(_suscribers
                    .Where(p => p.Key.IsAssignableFrom(oneEvent.GetType()))
                    .SelectMany(p => p.Value)
                    .Select(p => p.Value(oneEvent))
                    .ToList()
                );

        /// <summary>
        /// Subscribes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="suscriberId">The suscriber identifier.</param>
        /// <returns>Task.</returns>
        public Task<string> Subscribe<TEvent>(Func<IEvent, Task> action, string? suscriberId = null) where TEvent : IEvent
        {
            string id = suscriberId ?? Guid.NewGuid().ToBase64String();
            if (_suscribers.TryGetValue(typeof(TEvent), out Dictionary<string, Func<IEvent, Task>>? eventActions))
            {
                eventActions = new Dictionary<string, Func<IEvent, Task>>();
                _suscribers.Add(typeof(TEvent), eventActions);
            }
            eventActions?.Add(id, action);
            return Task.FromResult(id);
        }

        /// <summary>
        /// Unsubscribes the specified suscriber identifier.
        /// </summary>
        /// <param name="suscriberId">The suscriber identifier.</param>
        /// <returns>Task.</returns>
        public Task Unsubscribe<TEvent>(string suscriberId) where TEvent : IEvent
        {
            _suscribers[typeof(TEvent)].Remove(suscriberId);
            return Task.CompletedTask;
        }
    }
}
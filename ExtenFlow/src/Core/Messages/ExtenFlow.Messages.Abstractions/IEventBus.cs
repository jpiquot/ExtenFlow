using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExtenFlow.Messages
{
    /// <summary>
    /// Message receiver
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publishes the specified events.
        /// </summary>
        /// <param name="events">The events to publish.</param>
        Task Publish(IEnumerable<IEvent> events);

        /// <summary>
        /// Publishes the specified one event.
        /// </summary>
        /// <param name="oneEvent">The event to publish.</param>
        Task Publish(IEvent oneEvent);

        /// <summary>
        /// Subscribes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="suscriberId"></param>
        /// <returns>Task.</returns>
        Task<string> Subscribe<TEvent>(Func<IEvent, Task> action, string? suscriberId) where TEvent : IEvent;

        /// <summary>
        /// Unsubscribes the specified suscriber identifier.
        /// </summary>
        /// <param name="suscriberId">The suscriber identifier.</param>
        /// <returns>Task.</returns>
        Task Unsubscribe<TEvent>(string suscriberId) where TEvent : IEvent;
    }
}
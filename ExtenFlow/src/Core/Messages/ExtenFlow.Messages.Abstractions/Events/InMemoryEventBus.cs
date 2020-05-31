using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Events
{
    /// <summary>
    /// Class InMemoryEventBus. Implements the <see cref="ExtenFlow.Messages.Events.IEventPublisher"/>
    /// </summary>
    /// <remarks>Only used for testing.</remarks>
    /// <seealso cref="ExtenFlow.Messages.Events.IEventPublisher"/>
    public class InMemoryEventBus : IEventPublisher
    {
        private Dictionary<int, IList<IEvent>>? _messages;

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public Dictionary<int, IList<IEvent>> Messages => _messages ?? (_messages = new Dictionary<int, IList<IEvent>>());

        /// <summary>
        /// Publish an event 0-n handler functions.
        /// </summary>
        /// <param name="events">Event object to be published on the event bus.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ArgumentNullException">events</exception>
        public Task Publish(IList<IEvent> events, CancellationToken? cancellationToken = default)
        {
            _ = events ?? throw new ArgumentNullException(nameof(events));
            Messages.Add(Messages.Select(p => p.Key).LastOrDefault() + 1, events);
            return Task.CompletedTask;
        }
    }
}
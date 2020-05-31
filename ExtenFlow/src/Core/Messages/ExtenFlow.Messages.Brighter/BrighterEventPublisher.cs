using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ExtenFlow.Messages.Brighter.Requests;
using ExtenFlow.Messages.Events;

using Paramore.Brighter;

namespace ExtenFlow.Messages.Brighter
{
    /// <summary>
    /// Class BrighterEventPublisher. Implements the <see cref="ExtenFlow.Messages.Events.IEventPublisher"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.Events.IEventPublisher"/>
    public sealed class BrighterEventPublisher : IEventPublisher
    {
        private readonly IAmACommandProcessor _commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrighterEventPublisher"/> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public BrighterEventPublisher(IAmACommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        /// <summary>
        /// Publish an event 0-n handler functions.
        /// </summary>
        /// <param name="eventToPublish">Event object to be published on the event bus.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task.</returns>
        Task IEventPublisher.Publish(IList<IEvent> eventToPublish, CancellationToken? cancellationToken)
            => Task.WhenAll(
                eventToPublish
                    .Select(p => _commandProcessor.PublishAsync(new BrighterEvent(p)))
                    .ToList()
                );
    }
}
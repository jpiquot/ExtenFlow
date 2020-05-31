using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Events
{
    /// <summary>
    /// Defines an event publisher
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publish an event 0-n handler functions.
        /// </summary>
        /// <param name="events">Event object to be published on the event bus.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task Publish(IList<IEvent> events, CancellationToken? cancellationToken = default);
    }
}
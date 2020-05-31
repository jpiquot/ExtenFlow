using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Events
{
    /// <summary>
    ///Event store writer
    /// </summary>
    public interface IEventStoreWriter
    {
        /// <summary>
        /// Appends the specified events to the store stream.
        /// </summary>
        /// <param name="events">The events to persist.</param>
        /// <returns>The stored events version.</returns>
        Task<string> Append(IList<IEvent> events);
    }
}
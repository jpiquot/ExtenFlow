using System.Collections.Generic;
using System.Threading.Tasks;

using ExtenFlow.Messages;

namespace ExtenFlow.EventStorage
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
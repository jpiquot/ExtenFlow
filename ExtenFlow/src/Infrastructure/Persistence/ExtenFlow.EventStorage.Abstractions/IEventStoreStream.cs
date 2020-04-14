using System;
using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;

using ExtenFlow.Messages;

[assembly: NeutralResourcesLanguage("en")]

namespace ExtenFlow.EventStorage
{
    /// <summary>
    /// Message receiver
    /// </summary>
    public interface IEventStoreStream
    {
        /// <summary>
        /// Appends the specified events to the store stream.
        /// </summary>
        /// <param name="events">The events to persist.</param>
        Task Append(IList<IEvent> events);

        /// <summary>
        /// Reads the events from the store.
        /// </summary>
        /// <param name="afterId">
        /// Ignore all events before the event with the given identifier, including itself.
        /// </param>
        /// <param name="take">Maximum number of events to read.</param>
        /// <returns>The list of the matching events.</returns>
        Task<IList<IEvent>> Read(Guid? afterId = null, int take = 0);
    }
}
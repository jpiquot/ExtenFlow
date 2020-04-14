using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Dispatcher
{
    /// <summary>
    /// Message receiver
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Appends the specified events.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns>Task.</returns>
        Task Append(IList<IEvent> events);

        /// <summary>
        /// Reads all the events after the specified event identifier.
        /// </summary>
        /// <param name="afterId">The after identifier.</param>
        /// <param name="take">The take.</param>
        /// <returns>Task&lt;IList&lt;IEvent&gt;&gt;.</returns>
        Task<IList<IEvent>> Read(Guid? afterId = null, int take = 0);
    }
}
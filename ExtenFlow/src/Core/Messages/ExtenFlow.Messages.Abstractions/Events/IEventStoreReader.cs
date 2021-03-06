﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Events
{
    /// <summary>
    /// Event store reader
    /// </summary>
    public interface IEventStoreReader
    {
        /// <summary>
        /// Reads the events from the store.
        /// </summary>
        /// <param name="afterVersion">
        /// Ignore all events before the event with the given identifier, including itself.
        /// </param>
        /// <param name="take">Maximum number of events to read.</param>
        /// <returns>
        /// The list of the matching events or (null, null) if no more events where found.
        /// </returns>
        Task<(IList<IEvent>? events, string? version)> Read(string? afterVersion = null, int take = 0);
    }
}
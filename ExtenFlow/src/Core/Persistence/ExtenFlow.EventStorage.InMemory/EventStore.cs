using System.Collections.Generic;

namespace ExtenFlow.EventStorage.InMemory
{
    /// <summary>
    /// Class EventStore. Implements the <see cref="ExtenFlow.EventStorage.IEventStore"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.EventStorage.IEventStore"/>
    public class EventStore : IEventStore
    {
        private readonly Dictionary<string, IEventStoreStream> _streams = new Dictionary<string, IEventStoreStream>();

        /// <summary>
        /// Gets the store stream.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns>Task&lt;IEventStoreStream&gt;.</returns>
        public IEventStoreStream GetStream(string aggregateType, string aggregateId)
        {
            if (!_streams.TryGetValue(GetStreamId(aggregateType, aggregateId), out IEventStoreStream? stream))
            {
                stream = new EventStoreStream();
                _streams.Add(GetStreamId(aggregateType, aggregateId), stream);
            }
            return stream;
        }

        private static string GetStreamId(string aggregateType, string aggregateId) => $"{aggregateType}-[{aggregateId}]";
    }
}
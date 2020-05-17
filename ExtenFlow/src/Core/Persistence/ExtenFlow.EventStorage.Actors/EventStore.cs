namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Class EventStore. Implements the <see cref="ExtenFlow.EventStorage.IEventStore"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.EventStorage.IEventStore"/>
    public class EventStore : IEventStore
    {
        /// <summary>
        /// Gets the store stream.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns>Task&lt;IEventStoreStream&gt;.</returns>
        public static IEventStoreStream GetStream(string aggregateType, string aggregateId)
            => new EventStoreStream(aggregateType, aggregateId);

        IEventStoreStream IEventStore.GetStream(string aggregateType, string aggregateId)
            => GetStream(aggregateType, aggregateId);
    }
}
namespace ExtenFlow.EventStorage
{
    /// <summary>
    /// Message receiver
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Gets the store stream.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns>Task&lt;IEventStoreStream&gt;.</returns>
        IEventStoreStream GetStream(string aggregateType, string aggregateId);
    }
}
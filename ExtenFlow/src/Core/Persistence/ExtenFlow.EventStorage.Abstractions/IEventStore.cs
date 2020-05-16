using System.Threading.Tasks;

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
        Task<IEventStoreStream> GetStoreStream(string aggregateType, string aggregateId);
    }
}
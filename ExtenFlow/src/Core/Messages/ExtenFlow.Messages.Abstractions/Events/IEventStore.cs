namespace ExtenFlow.EventStorage
{
    /// <summary>
    /// The event store
    /// </summary>
    public interface IEventStore : IEventStoreReader, IEventStoreWriter
    {
    }
}
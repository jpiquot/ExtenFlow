namespace ExtenFlow.Messages.Events
{
    /// <summary>
    /// Interface IEventStore Implements the <see
    /// cref="ExtenFlow.Messages.Events.IEventStoreReader"/> Implements the <see cref="ExtenFlow.Messages.Events.IEventStoreWriter"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.Events.IEventStoreReader"/>
    /// <seealso cref="ExtenFlow.Messages.Events.IEventStoreWriter"/>
    public interface IEventStore : IEventStoreReader, IEventStoreWriter
    {
    }
}
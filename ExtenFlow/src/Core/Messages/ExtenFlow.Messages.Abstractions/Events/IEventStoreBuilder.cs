namespace ExtenFlow.Messages.Events
{
    /// <summary>
    /// Interface IEventPublisherBuilder
    /// </summary>
    public interface IEventStoreBuilder
    {
        /// <summary>
        /// Builds the <see cref="ExtenFlow.Messages.Events.IEventStore"/> instance.
        /// </summary>
        /// <returns>IEventPublisher.</returns>
        IEventStore Build();

        /// <summary>
        /// Sets the event store stream name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IEventStoreBuilder.</returns>
        IEventStoreBuilder Name(string name);
    }
}
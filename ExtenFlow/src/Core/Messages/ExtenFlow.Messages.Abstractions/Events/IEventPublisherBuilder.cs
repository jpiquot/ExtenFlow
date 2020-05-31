namespace ExtenFlow.Messages.Events
{
    /// <summary>
    /// Interface IEventPublisherBuilder
    /// </summary>
    public interface IEventPublisherBuilder
    {
        /// <summary>
        /// Builds the <see cref="ExtenFlow.Messages.Events.IEventPublisher"/> instance.
        /// </summary>
        /// <returns>IEventPublisher.</returns>
        IEventPublisher Build();
    }
}
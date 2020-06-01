namespace ExtenFlow.Messages.Events
{
    /// <summary>
    /// Class InMemoryEventPublisherBuilder. Implements the <see cref="ExtenFlow.Messages.Events.IEventPublisherBuilder"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.Events.IEventPublisherBuilder"/>
    public class InMemoryEventPublisherBuilder : IEventPublisherBuilder
    {
        /// <summary>
        /// Builds the <see cref="ExtenFlow.Messages.Events.IEventPublisher"/> instance.
        /// </summary>
        /// <returns>IEventPublisher.</returns>
        public IEventPublisher Build() => new InMemoryEventPublisher();
    }
}
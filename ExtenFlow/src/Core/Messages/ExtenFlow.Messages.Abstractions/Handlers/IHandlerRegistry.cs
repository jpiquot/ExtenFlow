namespace ExtenFlow.Messages.Handlers
{
    /// <summary>
    /// Defines a registry of message handlers
    /// </summary>
    public interface IHandlerRegistry
    {
        /// <summary>
        /// Adds a handler to the registry.
        /// </summary>
        /// <typeparam name="TMessage">The type of the t message.</typeparam>
        /// <typeparam name="THandler">The type of the t handler.</typeparam>
        void Add<TMessage, THandler>()
            where TMessage : class, IMessage
            where THandler : class, IMessageHandler<IMessage>;
    }
}
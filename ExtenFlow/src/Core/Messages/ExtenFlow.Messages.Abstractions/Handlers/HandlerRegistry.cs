using System;
using System.Collections.Generic;

namespace ExtenFlow.Messages.Handlers
{
    /// <summary>
    /// Message handler class. Implements the <see cref="ExtenFlow.Messages.Handlers.IHandlerRegistry"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.Handlers.IHandlerRegistry"/>
    public class HandlerRegistry : IHandlerRegistry
    {
        private Dictionary<Type, Type>? _handlers;

        /// <summary>
        /// Gets the handlers.
        /// </summary>
        /// <value>The handlers.</value>
        public Dictionary<Type, Type> Handlers => _handlers ?? (_handlers = new Dictionary<Type, Type>());

        /// <summary>
        /// Adds a handler to the registry.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        public virtual void Add<TMessage, THandler>()
            where TMessage : class, IMessage
            where THandler : class, IMessageHandler<IMessage>
            => Handlers.Add(typeof(TMessage), typeof(THandler));
    }
}
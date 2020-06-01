using System;

namespace ExtenFlow.Messages.Events
{
    /// <summary>
    /// Class InMemoryEventStoreBuilder. Implements the <see cref="ExtenFlow.Messages.Events.IEventStoreBuilder"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.Events.IEventStoreBuilder"/>
    public class InMemoryEventStoreBuilder : IEventStoreBuilder
    {
        private string? _name;

        /// <summary>
        /// Builds the <see cref="ExtenFlow.Messages.Events.IEventStore"/> instance.
        /// </summary>
        /// <returns>IEventPublisher.</returns>
        public IEventStore Build()
            => new InMemoryEventStore(_name ?? throw new InvalidOperationException(Messages.Properties.Resources.UndefinedEventStoreName));

        /// <summary>
        /// Sets the event store stream name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>IEventStoreBuilder.</returns>
        public IEventStoreBuilder Name(string name)
        {
            _name = name;
            return this;
        }
    }
}
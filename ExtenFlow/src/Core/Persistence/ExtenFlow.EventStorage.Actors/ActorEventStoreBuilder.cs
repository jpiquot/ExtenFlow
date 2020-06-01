using System;

using ExtenFlow.Messages.Events;

namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Class ActorEventStoreBuilder. Implements the <see cref="IEventStoreBuilder"/>
    /// </summary>
    /// <seealso cref="IEventStoreBuilder"/>
    public class ActorEventStoreBuilder : IEventStoreBuilder
    {
        private string? _name;

        /// <summary>
        /// Builds the <see cref="IEventStore"/> instance.
        /// </summary>
        /// <returns>IEventPublisher.</returns>
        public IEventStore Build()
            => new EventStore(_name ?? throw new InvalidOperationException(Messages.Properties.Resources.UndefinedEventStoreName));

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
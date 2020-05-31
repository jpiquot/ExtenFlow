using System;

using ExtenFlow.Infrastructure;

namespace ExtenFlow.Messages.Brighter.Requests
{
    /// <summary>
    /// Class BrighterEvent. Implements the <see cref="Paramore.Brighter.IEvent"/>
    /// </summary>
    /// <seealso cref="Paramore.Brighter.IEvent"/>
    public class BrighterEvent : Paramore.Brighter.IEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrighterEvent"/> class.
        /// </summary>
        /// <param name="domainEvent">The command.</param>
        /// <exception cref="System.ArgumentNullException">command</exception>
        public BrighterEvent(IEvent domainEvent)
        {
            Event = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));
            Id = domainEvent.Id.ToGuidOrDefault() ?? Guid.NewGuid();
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        public IEvent Event { get; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }
    }
}
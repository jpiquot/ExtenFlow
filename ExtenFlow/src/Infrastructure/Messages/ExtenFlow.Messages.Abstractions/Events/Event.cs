using System;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete
#pragma warning disable CA1716 // Identifiers should not match keywords

namespace ExtenFlow.Messages
{
    /// <summary>
    /// The base class for all Events
    /// </summary>
    public abstract class Event : Message, IEvent
    {
        /// <summary>
        /// The events base class constructor
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected Event()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="aggregateType">The aggregate that will handle or has handled the message.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        protected Event(string aggregateType, string? aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
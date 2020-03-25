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
        /// The event base class constrcutor
        /// </summary>
        /// <param name="aggregateType">The type of the aggregate</param>
        /// <param name="aggregateId">The id of the aggregate</param>
        /// <param name="userId">The user that submitted the event</param>
        /// <param name="correlationId">The correlation id that links all the messages together</param>
        /// <param name="messageId">The is of this event</param>
        /// <param name="dateTime">The date and time of the event</param>
        protected Event(string aggregateType, string? aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
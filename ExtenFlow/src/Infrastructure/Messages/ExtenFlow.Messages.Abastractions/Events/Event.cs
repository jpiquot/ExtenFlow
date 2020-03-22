using System;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete
#pragma warning disable CA1716 // Identifiers should not match keywords

namespace ExtenFlow.Messages
{
    public abstract class Event : Message, IEvent
    {
        [Obsolete("Can only be used by serializers")]
        protected Event()
        {
        }

        protected Event(string aggregateType, string? aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
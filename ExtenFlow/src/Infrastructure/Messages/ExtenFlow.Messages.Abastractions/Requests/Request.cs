using System;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages
{
    public abstract class Request : Message, IRequest
    {
        [Obsolete("Can only be used by serializers")]
        protected Request()
        {
            AggregateId = string.Empty;
            AggregateType = string.Empty;
        }

        protected Request(string aggregateType, string? aggregateId, string? userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
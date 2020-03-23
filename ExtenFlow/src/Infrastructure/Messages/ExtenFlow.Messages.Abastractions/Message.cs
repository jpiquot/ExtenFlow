using System;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages
{
    public abstract class Message : IMessage
    {
        [Obsolete("Can only be used by serializers")]
        protected Message()
        {
            UserId = string.Empty;
            CorrelationId = Guid.NewGuid();
            DateTime = DateTimeOffset.Now;
            MessageId = Guid.NewGuid();
            AggregateType = string.Empty;
            AggregateId = null;
        }

        protected Message(string aggregateType, string? aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (string.IsNullOrWhiteSpace(aggregateType))
            {
                throw new ArgumentNullException(nameof(aggregateType));
            }
            if (correlationId == null || correlationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(correlationId));
            }
            if (messageId == null || messageId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(messageId));
            }
            UserId = userId;
            CorrelationId = correlationId;
            DateTime = dateTime;
            MessageId = messageId;
            AggregateId = aggregateId;
            AggregateType = aggregateType;
        }

        public Guid MessageId { get; [Obsolete]set; }
        public Guid CorrelationId { get; [Obsolete]set; }
        public DateTimeOffset DateTime { get; [Obsolete]set; }
        public string UserId { get; [Obsolete]set; }
        public string AggregateType { get; [Obsolete]set; }
        public string? AggregateId { get; [Obsolete]set; }
    }
}
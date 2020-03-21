using System;

using Newtonsoft.Json;

namespace ExtenFlow.Messages
{
    public abstract class Message : IMessage
    {
        [Obsolete("Can only be used by serializers", true)]
        protected Message()
        {
            UserId = string.Empty;
            CorrelationId = Guid.Empty;
            DateTime = DateTimeOffset.Now;
            MessageId = Guid.Empty;
        }

        protected Message(string userId, Guid correlationId) : this(userId, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        protected Message(string userId) : this(userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        [JsonConstructor]
        protected Message(string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
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
        }

        public Guid MessageId { get; }
        public Guid CorrelationId { get; }
        public DateTimeOffset DateTime { get; }
        public string UserId { get; }
    }
}
using System;

using Newtonsoft.Json;

namespace ExtenFlow.Messages
{
    public abstract class Request : Message, IRequest
    {
        [Obsolete("Can only be used by serializers", true)]
        protected Request()
        {
            AggregateId = string.Empty;
            AggregateType = string.Empty;
        }

        protected Request(string aggregateType, string aggregateId, string userId, Guid correlationId) : this(aggregateType, aggregateId, userId, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        protected Request(string aggregateType, string aggregateId, string userId) : this(aggregateType, aggregateId, userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        [JsonConstructor]
        protected Request(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(userId, correlationId, messageId, dateTime)
        {
            if (string.IsNullOrWhiteSpace(aggregateType))
            {
                throw new ArgumentNullException(nameof(aggregateType));
            }
            AggregateType = aggregateType;
            AggregateId = aggregateId;
        }

        public string AggregateType { get; }
        public string? AggregateId { get; }
    }
}
using System;

using Newtonsoft.Json;

namespace ExtenFlow.Messages
{
    public abstract class Command : Request, ICommand
    {
        [Obsolete("Can only be used by serializers", true)]
        protected Command()
        {
        }

        protected Command(string aggregateType, string aggregateId, string userId, Guid correlationId) : this(aggregateType, aggregateId, userId, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        protected Command(string aggregateType, string aggregateId, string userId) : this(aggregateType, aggregateId, userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        [JsonConstructor]
        protected Command(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
using System;

using Newtonsoft.Json;

namespace ExtenFlow.Messages
{
#pragma warning disable CA1716 // Identifiers should not match keywords

    public abstract class Event : Message, IEvent
#pragma warning restore CA1716 // Identifiers should not match keywords
    {
        [Obsolete("Can only be used by serializers", true)]
        protected Event()
        {
        }

        protected Event(string userId, Guid correlationId) : this(userId, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        [JsonConstructor]
        protected Event(string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(userId, correlationId, messageId, dateTime)
        {
        }
    }
}
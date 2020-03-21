using System;

namespace ExtenFlow.Messages
{
    public interface IMessage
    {
        Guid MessageId { get; }
        Guid CorrelationId { get; }

        DateTimeOffset DateTime { get; }
        string UserId { get; }
    }
}
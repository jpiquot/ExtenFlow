using System;
using System.Collections.Generic;

using Newtonsoft.Json;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class TestRequest : Request
    {
        [Obsolete]
        public TestRequest()
        {
        }

        [JsonConstructor]
        public TestRequest(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public abstract class RequestBaseTest<T> : MessageBaseTest<T> where T : IRequest
    {
    }

    public class RequestTest : RequestBaseTest<TestRequest>
    {
        protected override IEnumerable<TestRequest> Create(IDictionary<string, object> values)
        {
            var message = new TestRequest();
            message.AggregateType = (string)values[nameof(IMessage.AggregateType)];
            message.AggregateId = (string)values[nameof(IMessage.AggregateId)];
            message.UserId = (string)values[nameof(IMessage.UserId)];
            message.CorrelationId = (Guid)values[nameof(IMessage.CorrelationId)];
            message.MessageId = (Guid)values[nameof(IMessage.MessageId)];
            message.DateTime = (DateTimeOffset)values[nameof(IMessage.DateTime)];

            return new TestRequest[]{
                new TestRequest(
                    (string)values[nameof(IMessage.AggregateType)],
                    (string)values[nameof(IMessage.AggregateId)],
                    (string)values[nameof(IMessage.UserId)],
                    (Guid)values[nameof(IMessage.CorrelationId)],
                    (Guid)values[nameof(IMessage.MessageId)],
                    (DateTimeOffset)values[nameof(IMessage.DateTime)]
                ) };
        }
    }
}
using System;
using System.Collections.Generic;

using Newtonsoft.Json;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class TestQuery : Query<string>
    {
        [Obsolete]
        public TestQuery()
        {
        }

        [JsonConstructor]
        public TestQuery(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public abstract class QueryBaseTest<TR, TQ> : RequestBaseTest<TQ> where TQ : IQuery<TR>
    {
    }

    public class QueryTest : QueryBaseTest<string, TestQuery>
    {
        protected override IEnumerable<TestQuery> Create(IDictionary<string, object> values)
        {
            var message = new TestQuery();
            message.AggregateType = (string)values[nameof(IMessage.AggregateType)];
            message.AggregateId = (string)values[nameof(IMessage.AggregateId)];
            message.UserId = (string)values[nameof(IMessage.UserId)];
            message.CorrelationId = (Guid)values[nameof(IMessage.CorrelationId)];
            message.MessageId = (Guid)values[nameof(IMessage.MessageId)];
            message.DateTime = (DateTimeOffset)values[nameof(IMessage.DateTime)];

            return new TestQuery[]{
                new TestQuery(
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
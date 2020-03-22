using System;
using System.Collections.Generic;

using ExtenFlow.Security.Users.Queries;

using Newtonsoft.Json;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class TestUserQuery : UserQuery<string>
    {
        [Obsolete]
        public TestUserQuery()
        {
        }

        [JsonConstructor]
        public TestUserQuery(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public abstract class UserQueryBaseTest<TR, TQ> : QueryBaseTest<TR, TQ> where TQ : UserQuery<TR>
    {
        protected override Dictionary<string, object> GetTestValues()
        {
            var values = base.GetTestValues();
            values[nameof(IMessage.AggregateType)] = "User";
            return values;
        }
    }

    public class QueryTest : QueryBaseTest<string, TestUserQuery>
    {
        protected override IEnumerable<TestUserQuery> Create(IDictionary<string, object> values)
        {
            var message = new TestUserQuery();
            message.AggregateId = (string)values[nameof(IMessage.AggregateId)];
            message.UserId = (string)values[nameof(IMessage.UserId)];
            message.CorrelationId = (Guid)values[nameof(IMessage.CorrelationId)];
            message.MessageId = (Guid)values[nameof(IMessage.MessageId)];
            message.DateTime = (DateTimeOffset)values[nameof(IMessage.DateTime)];

            return new TestUserQuery[]{message,
                new TestUserQuery(
                    (string)values[nameof(IMessage.AggregateId)],
                    (string)values[nameof(IMessage.UserId)],
                    (Guid)values[nameof(IMessage.CorrelationId)],
                    (Guid)values[nameof(IMessage.MessageId)],
                    (DateTimeOffset)values[nameof(IMessage.DateTime)]
                ) };
        }
    }
}
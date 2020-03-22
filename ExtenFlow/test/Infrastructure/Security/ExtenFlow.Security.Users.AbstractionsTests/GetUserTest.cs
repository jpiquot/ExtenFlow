using System;
using System.Collections.Generic;

using ExtenFlow.Messages;
using ExtenFlow.Messages.AbstractionsTests;
using ExtenFlow.Security.Users.Queries;

namespace ExtenFlow.Security.Users.AbstractionsTests
{
    public class GetUserTest : UserQueryBaseTest<IUser, GetUser>
    {
        protected override IEnumerable<GetUser> Create(IDictionary<string, object> values)
        {
            string aggregateId = (string)values[nameof(IMessage.AggregateId)];
            var correlationId = (Guid)values[nameof(IMessage.CorrelationId)];
            var messageId = (Guid)values[nameof(IMessage.MessageId)];
            var dateTime = (DateTimeOffset)values[nameof(IMessage.DateTime)];

            var list = new GetUser[]{
                new GetUser(aggregateId, correlationId, messageId, dateTime)
            };
            return list;
        }
    }
}
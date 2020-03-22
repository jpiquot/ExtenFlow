using System;
using System.Collections.Generic;

using ExtenFlow.Messages;
using ExtenFlow.Messages.AbstractionsTests;
using ExtenFlow.Security.Users.Queries;

using FluentAssertions;

namespace ExtenFlow.Security.Users.AbstractionsTests
{
    public class GetAuthenticatedUserTest : UserQueryBaseTest<IUser, GetAuthenticatedUser>
    {
        protected override Dictionary<string, object> GetTestValues()
        {
            var values = base.GetTestValues();
            values.Add(nameof(GetAuthenticatedUser.Password), "Test password@256");
            return values;
        }

        protected override IEnumerable<GetAuthenticatedUser> Create(IDictionary<string, object> values)
        {
            string aggregateId = (string)values[nameof(IMessage.AggregateId)];
            string password = (string)values[nameof(IMessage.AggregateId)];
            var correlationId = (Guid)values[nameof(IMessage.CorrelationId)];
            var messageId = (Guid)values[nameof(IMessage.MessageId)];
            var dateTime = (DateTimeOffset)values[nameof(IMessage.DateTime)];

            var list = new GetAuthenticatedUser[]{
                new GetAuthenticatedUser(aggregateId, password),
                new GetAuthenticatedUser(aggregateId, password, correlationId, messageId, dateTime)
            };
            return list;
        }

        protected override GetAuthenticatedUser CheckMessageStateValues(GetAuthenticatedUser message, IDictionary<string, object> values)
        {
            base.CheckMessageStateValues(message, values);
            message.Password.Should().Be((string)values[nameof(GetAuthenticatedUser.Password)]);
            return message;
        }
    }
}
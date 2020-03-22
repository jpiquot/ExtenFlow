using System;
using System.Collections.Generic;
using System.Security.Claims;

using ExtenFlow.Messages;
using ExtenFlow.Messages.AbstractionsTests;
using ExtenFlow.Security.Users.Queries;

using FluentAssertions;

namespace ExtenFlow.Security.Users.AbstractionsTests
{
    public class GetAuthenticatedClaimUserTest : UserQueryBaseTest<IUser, GetAuthenticatedClaimUser>
    {
        protected override Dictionary<string, object> GetTestValues()
        {
            var values = base.GetTestValues();
            values.Add(nameof(GetAuthenticatedClaimUser.Claim), GetUserPrincipal((string)values[nameof(IMessage.AggregateId)]));
            return values;
        }

        protected override IEnumerable<GetAuthenticatedClaimUser> Create(IDictionary<string, object> values)
        {
            var claim = (ClaimsPrincipal)values[nameof(GetAuthenticatedClaimUser.Claim)];
            var correlationId = (Guid)values[nameof(IMessage.CorrelationId)];
            var messageId = (Guid)values[nameof(IMessage.MessageId)];
            var dateTime = (DateTimeOffset)values[nameof(IMessage.DateTime)];
            var list = new GetAuthenticatedClaimUser[]{
                new GetAuthenticatedClaimUser(claim),
                new GetAuthenticatedClaimUser(claim, correlationId),
                new GetAuthenticatedClaimUser(claim, correlationId, messageId, dateTime)
            };
            return list;
        }

        protected override GetAuthenticatedClaimUser CheckMessageStateValues(GetAuthenticatedClaimUser message, IDictionary<string, object> values)
        {
            base.CheckMessageStateValues(message, values);
            message.Claim.Should().Be((ClaimsPrincipal)values[nameof(GetAuthenticatedClaimUser.Claim)]);
            return message;
        }

        private static ClaimsPrincipal GetUserPrincipal(string name)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, "Test")
            };

            var userIdentity = new ClaimsIdentity(claims, "Passport");

            var userPrincipal = new ClaimsPrincipal(userIdentity);
            return userPrincipal;
        }
    }
}
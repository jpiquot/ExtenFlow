using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

using ExtenFlow.Messages.AbstractionsTests;
using ExtenFlow.Security.Users.Queries;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.Security.Users.AbstractionsTests
{
    public class GetAuthenticatedClaimUserTest : IClassFixture<UserQueryFixture<IUser, GetAuthenticatedClaimUser>>
    {
        private UserQueryFixture<IUser, GetAuthenticatedClaimUser> GetAuthenticatedClaimUserFixture { get; }

        public GetAuthenticatedClaimUserTest(UserQueryFixture<IUser, GetAuthenticatedClaimUser> getAuthenticatedClaimUserFixture)
        {
            GetAuthenticatedClaimUserFixture = getAuthenticatedClaimUserFixture;
        }

        [Fact]
        public void CreateGetAuthenticatedClaimUser_EmptyMessageIdShouldThrowException()
            => Invoking(() => new GetAuthenticatedClaimUser(GetUserTestData.GetUserPrincipal("i am a user"), Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateGetAuthenticatedClaimUser_DefaultMessageShouldHaveAMessageId()
            => new GetAuthenticatedClaimUser(GetUserTestData.GetUserPrincipal("i am a user")).MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateGetAuthenticatedClaimUser_DefaultMessageShouldHaveACorrelationId()
            => new GetAuthenticatedClaimUser(GetUserTestData.GetUserPrincipal("i am a user")).CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateGetAuthenticatedClaimUser_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new GetAuthenticatedClaimUser(GetUserTestData.GetUserPrincipal("i am a user"), Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(GetUserTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(ClaimsPrincipal principal, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetAuthenticatedClaimUserFixture.CheckMessageNewtonSoftSerialization(new GetAuthenticatedClaimUser(principal, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(GetUserTestData))]
        public void DotNetJsonSerializeMessage_Check(ClaimsPrincipal principal, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetAuthenticatedClaimUserFixture.CheckMessageJsonSerialization(new GetAuthenticatedClaimUser(principal, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(GetUserTestData))]
        public void CreateGetAuthenticatedClaimUser_CheckState(ClaimsPrincipal principal, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetAuthenticatedClaimUserFixture.CheckMessageState(new GetAuthenticatedClaimUser(principal, correlationId, messageId, dateTime), "User", principal.Identity.Name, principal.Identity.Name, correlationId, messageId, dateTime);
    }

    public class GetUserTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { GetUserTestData.GetUserPrincipal("i am a user"), Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { GetUserTestData.GetUserPrincipal("I Am A User"), Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static ClaimsPrincipal GetUserPrincipal(string name)
        {
            var claims = new List<Claim> { new
 Claim(ClaimTypes.Name, name, ClaimValueTypes.String, "Test") };

            var userIdentity = new ClaimsIdentity(claims, "Passport");

            var userPrincipal = new ClaimsPrincipal(userIdentity);
            return userPrincipal;
        }
    }
}
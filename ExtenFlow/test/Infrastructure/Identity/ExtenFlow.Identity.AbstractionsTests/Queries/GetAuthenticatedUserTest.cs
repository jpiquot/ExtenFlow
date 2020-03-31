using System;
using System.Collections;
using System.Collections.Generic;

using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Queries;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class GetAuthenticatedUserTest : IClassFixture<UserQueryFixture<User, GetAuthenticatedUser>>
    {
        public GetAuthenticatedUserTest(UserQueryFixture<User, GetAuthenticatedUser> getAuthenticatedUserFixture)
        {
            GetAuthenticatedUserFixture = getAuthenticatedUserFixture;
        }

        private UserQueryFixture<User, GetAuthenticatedUser> GetAuthenticatedUserFixture { get; }

        [Theory]
        [ClassData(typeof(GetUserTestData))]
        public void CreateGetAuthenticatedUser_CheckState(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetAuthenticatedUserFixture.CheckMessageState(new GetAuthenticatedUser(aggregateId, password, userId, correlationId, messageId, dateTime), "User", aggregateId, userId, correlationId, messageId, dateTime);

        [Fact]
        public void CreateGetAuthenticatedUser_DefaultMessageShouldHaveACorrelationId()
            => new GetAuthenticatedUser("aggr id", "pass").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateGetAuthenticatedUser_DefaultMessageShouldHaveAMessageId()
            => new GetAuthenticatedUser("aggr id", "pass").MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateGetAuthenticatedUser_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new GetAuthenticatedUser("Aggr. Id", "pass", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateGetAuthenticatedUser_EmptyMessageIdShouldThrowException()
            => Invoking(() => new GetAuthenticatedUser("Aggr. Id", "pass", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateGetAuthenticatedUser_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new GetAuthenticatedUser("Aggr. Id", "pass", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(GetUserTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetAuthenticatedUserFixture.CheckMessageJsonSerialization(new GetAuthenticatedUser(aggregateId, password, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(GetUserTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetAuthenticatedUserFixture.CheckMessageNewtonSoftSerialization(new GetAuthenticatedUser(aggregateId, password, userId, correlationId, messageId, dateTime));
    }

    public class GetUserTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "passWord @ 123", "User Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. ID", "passWord123/%$", "USER", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
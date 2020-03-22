using System;

using ExtenFlow.Security.Users;
using ExtenFlow.Security.Users.Queries;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class GetUserTest : IClassFixture<UserQueryFixture<IUser, GetUser>>
    {
        private UserQueryFixture<IUser, GetUser> GetUserFixture { get; }

        public GetUserTest(UserQueryFixture<IUser, GetUser> getUserFixture)
        {
            GetUserFixture = GetUserFixture;
        }

        [Fact]
        public void CreateGetUser_EmptyMessageIdShouldThrowException()
            => Invoking(() => new GetUser("Aggr. Id", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateGetUser_DefaultMessageShouldHaveAMessageId()
            => new GetUser("aggr id").MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateGetUser_DefaultMessageShouldHaveACorrelationId()
            => new GetUser("aggr id").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateGetUser_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new GetUser("Aggr. Id", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateGetUser_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new GetUser("Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(UserQueryTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetUserFixture.CheckMessageNewtonSoftSerialization(new GetUser(aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(UserQueryTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetUserFixture.CheckMessageJsonSerialization(new GetUser(aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(UserQueryTestData))]
        public void CreateGetUser_CheckState(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetUserFixture.CheckMessageState(new GetUser(aggregateId, userId, correlationId, messageId, dateTime), "User", aggregateId, userId, correlationId, messageId, dateTime);
    }
}
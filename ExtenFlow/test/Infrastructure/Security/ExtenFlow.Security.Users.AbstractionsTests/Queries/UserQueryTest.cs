using System;

using ExtenFlow.Security.Users.Queries;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class FakeUserQuery : UserQuery<string>
    {
        [Obsolete]
        public FakeUserQuery()
        {
        }

        [JsonConstructor]
        public FakeUserQuery(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public class UserQueryTest : IClassFixture<UserQueryFixture<string, FakeUserQuery>>
    {
        private UserQueryFixture<string, FakeUserQuery> UserQueryFixture { get; }

        public UserQueryTest(UserQueryFixture<string, FakeUserQuery> userQueryFixture)
        {
            UserQueryFixture = userQueryFixture;
        }

        [Fact]
        public void CreateUserQuery_EmptyMessageIdShouldThrowException()
            => Invoking(() => new FakeUserQuery("Aggr. Id", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateUserQuery_DefaultMessageShouldHaveAMessageId()
            => new FakeUserQuery().MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateUserQuery_DefaultMessageShouldHaveACorrelationId()
            => new FakeUserQuery().CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateUserQuery_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeUserQuery("Aggr. Id", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateUserQuery_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new FakeUserQuery("Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(UserQueryTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => UserQueryFixture.CheckMessageNewtonSoftSerialization(new FakeUserQuery(aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(UserQueryTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => UserQueryFixture.CheckMessageJsonSerialization(new FakeUserQuery(aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(UserQueryTestData))]
        public void CreateUserQuery_CheckState(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => UserQueryFixture.CheckMessageState(new FakeUserQuery(aggregateId, userId, correlationId, messageId, dateTime), "User", aggregateId, userId, correlationId, messageId, dateTime);
    }
}
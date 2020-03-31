using System;

using ExtenFlow.Identity.Users.Commands;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class FakeUserCommand : UserCommand
    {
        [Obsolete]
        public FakeUserCommand()
        {
        }

        [JsonConstructor]
        public FakeUserCommand(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public class UserCommandTest : IClassFixture<UserCommandFixture<FakeUserCommand>>
    {
        public UserCommandTest(UserCommandFixture<FakeUserCommand> userCommandFixture)
        {
            UserCommandFixture = userCommandFixture;
        }

        private UserCommandFixture<FakeUserCommand> UserCommandFixture { get; }

        [Theory]
        [ClassData(typeof(UserCommandTestData))]
        public void CreateUserCommand_CheckState(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => UserCommandFixture.CheckMessageState(new FakeUserCommand(aggregateId, userId, correlationId, messageId, dateTime), "User", aggregateId, userId, correlationId, messageId, dateTime);

        [Fact]
        public void CreateUserCommand_DefaultMessageShouldHaveACorrelationId()
            => new FakeUserCommand().CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateUserCommand_DefaultMessageShouldHaveAMessageId()
            => new FakeUserCommand().MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateUserCommand_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeUserCommand("Aggr. Id", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateUserCommand_EmptyMessageIdShouldThrowException()
            => Invoking(() => new FakeUserCommand("Aggr. Id", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateUserCommand_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new FakeUserCommand("Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(UserCommandTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => UserCommandFixture.CheckMessageJsonSerialization(new FakeUserCommand(aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(UserCommandTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => UserCommandFixture.CheckMessageNewtonSoftSerialization(new FakeUserCommand(aggregateId, userId, correlationId, messageId, dateTime));
    }
}
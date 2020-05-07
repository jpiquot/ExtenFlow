using System;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class CommandTest : IClassFixture<CommandFixture<FakeCommand>>
    {
        public CommandTest(CommandFixture<FakeCommand> messageFixture)
        {
            MessageFixture = messageFixture;
        }

        private CommandFixture<FakeCommand> MessageFixture { get; }

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void CreateCommand_CheckState(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageState(new FakeCommand(aggregateType, aggregateId, userId, correlationId, id, dateTime), aggregateType, aggregateId, userId, correlationId, id, dateTime);

        [Fact]
        public void CreateCommand_DefaultMessageShouldHaveACorrelationId()
            => new FakeCommand().CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateCommand_DefaultMessageShouldHaveAId()
            => new FakeCommand().Id
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateCommand_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeCommand("Aggr. Type", "Aggr. Id", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateCommand_EmptyIdShouldThrowException()
            => Invoking(() => new FakeCommand("Aggr. Type", "Aggr. Id", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateCommand_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new FakeCommand("Aggr. Type", "Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageJsonSerialization(new FakeCommand(aggregateType, aggregateId, userId, correlationId, id, dateTime));

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageNewtonSoftSerialization(new FakeCommand(aggregateType, aggregateId, userId, correlationId, id, dateTime));
    }

    public class FakeCommand : Command
    {
        [Obsolete]
        public FakeCommand()
        {
        }

        [JsonConstructor]
        public FakeCommand(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, null, userId, correlationId, id, dateTime)
        {
        }
    }
}
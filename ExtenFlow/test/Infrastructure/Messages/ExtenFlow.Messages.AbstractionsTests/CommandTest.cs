using System;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

namespace ExtenFlow.Messages.AbstractionsTests
{
    internal class TestCommand : Command
    {
        [Obsolete]
        public TestCommand()
        {
        }

        public TestCommand(string aggregateType, string? aggregateId, string userId) : base(aggregateType, aggregateId, userId)
        {
        }

        public TestCommand(string aggregateType, string? aggregateId, string userId, Guid correlationId) : base(aggregateType, aggregateId, userId, correlationId)
        {
        }

        [JsonConstructor]
        public TestCommand(string aggregateType, string? aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public class CommandBaseTest : RequestBaseTest
    {
        protected T CheckCommandNewtonSonfSerialization<T>(T message) where T : ICommand

        {
            T deserializedMessage = CheckRequestNewtonSonfSerialization(message);
            return deserializedMessage;
        }
    }

    public class CommandTest : CommandBaseTest
    {
        [Theory]
        [InlineData("testUser")]
        [InlineData("a")]
        [InlineData("ekjAfkd/§?klazlkzaklzalkzakl")]
        public void CheckCommandAggregateTypeState(string aggregateType)
        {
            var message = new TestCommand(aggregateType, "id", "userId");
            message.AggregateType.Should().Be(aggregateType);
            message = new TestCommand(aggregateType, "id", "userId", Guid.NewGuid());
            message.AggregateType.Should().Be(aggregateType);
            message = new TestCommand(aggregateType, "id", "userId", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
            message.AggregateType.Should().Be(aggregateType);
        }

        [Fact]
        public void CheckCommandBaseState()
        {
            const string userId = "toto";
            var correlationId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            DateTimeOffset dateTime = DateTimeOffset.Now;
            var message = new TestCommand("titixx", "tutuxx", userId, correlationId, messageId, dateTime);
            message.UserId.Should().Be(userId);
            message.CorrelationId.Should().Be(correlationId);
            message.MessageId.Should().Be(messageId);
            message.DateTime.Should().Be(dateTime);
        }

        [Fact]
        public void CheckNewtonSoftJsonSerializeAndDesiarializeCommand()
        {
            var message = new TestCommand("AgrType", "Agr id", "Tu To Ti", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);

            CheckCommandNewtonSonfSerialization(message);
        }
    }
}
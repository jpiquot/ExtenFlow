using System;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.Messages.AbstractionsTests
{
    internal class TestRequest : Request
    {
        [Obsolete]
        public TestRequest()
        {
        }

        public TestRequest(string aggregateType, string aggregateId, string userId) : base(aggregateType, aggregateId, userId)
        {
        }

        public TestRequest(string aggregateType, string aggregateId, string userId, Guid correlationId) : base(aggregateType, aggregateId, userId, correlationId)
        {
        }

        [JsonConstructor]
        public TestRequest(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public class RequestBaseTest : MessageBaseTest
    {
        protected T CheckRequestNewtonSonfSerialization<T>(T message) where T : IRequest

        {
            T deserializedMessage = CheckMessageNewtonSonfSerialization(message);
            deserializedMessage.AggregateId.Should().Be(message.AggregateId);
            deserializedMessage.AggregateType.Should().Be(message.AggregateType);
            return deserializedMessage;
        }
    }

    public class RequestTest : RequestBaseTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void UndefinedAggregateTypedShouldThrowException(string aggregateType)
             => Invoking(() => new TestRequest(aggregateType, "tutuxx", "user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                 .Should()
                 .Throw<ArgumentNullException>();

        [Theory]
        [InlineData("testUser")]
        [InlineData("a")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("       ")]
        [InlineData("ekjAfkd/§?klazlkzaklzalkzakl")]
        public void CheckRequestAggregateIdState(string aggregateId)
        {
            var message = new TestRequest("tutuxx", aggregateId, "userId");
            message.AggregateId.Should().Be(aggregateId);
            message = new TestRequest("tutuxx", aggregateId, "userId", Guid.NewGuid());
            message.AggregateId.Should().Be(aggregateId);
            message = new TestRequest("tutuxx", aggregateId, "userId", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
            message.AggregateId.Should().Be(aggregateId);
        }

        [Theory]
        [InlineData("testUser")]
        [InlineData("a")]
        [InlineData("ekjAfkd/§?klazlkzaklzalkzakl")]
        public void CheckRequestAggregateTypeState(string aggregateType)
        {
            var message = new TestRequest(aggregateType, "id", "userId");
            message.AggregateType.Should().Be(aggregateType);
            message = new TestRequest(aggregateType, "id", "userId", Guid.NewGuid());
            message.AggregateType.Should().Be(aggregateType);
            message = new TestRequest(aggregateType, "id", "userId", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
            message.AggregateType.Should().Be(aggregateType);
        }

        [Fact]
        public void CheckRequestBaseState()
        {
            const string userId = "toto";
            var correlationId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            DateTimeOffset dateTime = DateTimeOffset.Now;
            var message = new TestRequest("titixx", "tutuxx", userId, correlationId, messageId, dateTime);
            message.UserId.Should().Be(userId);
            message.CorrelationId.Should().Be(correlationId);
            message.MessageId.Should().Be(messageId);
            message.DateTime.Should().Be(dateTime);
        }

        [Fact]
        public void CheckNewtonSoftJsonSerializeAndDesiarializeRequest()
        {
            var message = new TestRequest("AgrType", "Agr id", "Tu To Ti", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);

            CheckRequestNewtonSonfSerialization(message);
        }
    }
}
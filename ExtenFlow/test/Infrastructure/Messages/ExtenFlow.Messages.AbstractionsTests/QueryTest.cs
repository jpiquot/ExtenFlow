using System;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

namespace ExtenFlow.Messages.AbstractionsTests
{
    internal class TestQuery<T> : Query<T>
    {
        [Obsolete]
        public TestQuery()
        {
        }

        public TestQuery(string aggregateType, string? aggregateId, string userId) : base(aggregateType, aggregateId, userId)
        {
        }

        public TestQuery(string aggregateType, string? aggregateId, string userId, Guid correlationId) : base(aggregateType, aggregateId, userId, correlationId)
        {
        }

        [JsonConstructor]
        public TestQuery(string aggregateType, string? aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public class QueryBaseTest : RequestBaseTest
    {
        protected T CheckQueryNewtonSonfSerialization<T, S>(T message) where T : IQuery<S>

        {
            T deserializedMessage = CheckRequestNewtonSonfSerialization(message);
            return deserializedMessage;
        }
    }

    public class QueryTest : QueryBaseTest
    {
        [Theory]
        [InlineData("testUser")]
        [InlineData("a")]
        [InlineData("ekjAfkd/§?klazlkzaklzalkzakl")]
        public void CheckQueryAggregateTypeState(string aggregateType)
        {
            var message = new TestQuery(aggregateType, "id", "userId");
            message.AggregateType.Should().Be(aggregateType);
            message = new TestQuery(aggregateType, "id", "userId", Guid.NewGuid());
            message.AggregateType.Should().Be(aggregateType);
            message = new TestQuery(aggregateType, "id", "userId", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
            message.AggregateType.Should().Be(aggregateType);
        }

        [Fact]
        public void CheckQueryBaseState()
        {
            const string userId = "toto";
            var correlationId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            DateTimeOffset dateTime = DateTimeOffset.Now;
            var message = new TestQuery("titixx", "tutuxx", userId, correlationId, messageId, dateTime);
            message.UserId.Should().Be(userId);
            message.CorrelationId.Should().Be(correlationId);
            message.MessageId.Should().Be(messageId);
            message.DateTime.Should().Be(dateTime);
        }

        [Fact]
        public void CheckNewtonSoftJsonSerializeAndDesiarializeQuery()
        {
            var message = new TestQuery("AgrType", "Agr id", "Tu To Ti", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);

            CheckQueryNewtonSonfSerialization(message);
        }
    }
}
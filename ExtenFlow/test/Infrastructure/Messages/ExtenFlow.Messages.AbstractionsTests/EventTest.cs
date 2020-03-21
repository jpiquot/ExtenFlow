using System;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

namespace ExtenFlow.Messages.AbstractionsTests
{
    internal class TestEvent : Event
    {
        [Obsolete]
        public TestEvent()
        {
        }

        public TestEvent(string userId, Guid correlationId) : base(userId, correlationId)
        {
        }

        [JsonConstructor]
        public TestEvent(string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(userId, correlationId, messageId, dateTime)
        {
        }
    }

    public class EventBaseTest : MessageBaseTest
    {
        protected T CheckEventNewtonSonfSerialization<T>(T message) where T : IEvent

        {
            var deserializedMessage = CheckMessageNewtonSonfSerialization(message);
            return deserializedMessage;
        }
    }

    public class EventTest : EventBaseTest
    {
        [Fact]
        public void CheckEventBaseState()
        {
            const string userId = "toto";
            var correlationId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            DateTimeOffset dateTime = DateTimeOffset.Now;
            var message = new TestEvent(userId, correlationId, messageId, dateTime);
            message.UserId.Should().Be(userId);
            message.CorrelationId.Should().Be(correlationId);
            message.MessageId.Should().Be(messageId);
            message.DateTime.Should().Be(dateTime);
        }

        [Fact]
        public void CheckNewtonSoftJsonSerializeAndDesiarializeEvent()
        {
            var @event = new TestEvent("Tu To Ti", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);

            CheckEventNewtonSonfSerialization(@event);
        }
    }
}
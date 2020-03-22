using System;
using System.Text.Json;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class TestMessage : Message
    {
        [Obsolete]
        public TestMessage()
        {
        }

        public TestMessage(string userId) : base(userId)
        {
        }

        public TestMessage(string userId, Guid correlationId) : base(userId, correlationId)
        {
        }

        [JsonConstructor]
        public TestMessage(string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(userId, correlationId, messageId, dateTime)
        {
        }
    }

    public class MessageBaseTest<T> where T : IMessage
    {
        protected T CheckMessageNewtonSoftSerialization(T message)
        {
            string json = JsonConvert.SerializeObject(message);
            T deserializedMessage = JsonConvert.DeserializeObject<T>(json);
            CheckMessageStateAreEqual(message, deserializedMessage);
            return deserializedMessage;
        }

        protected T CheckMessageJsonSerialization(T message)
        {
            string json = System.Text.Json.JsonSerializer.Serialize<T>(message);
            T deserializedMessage = System.Text.Json.JsonSerializer.Deserialize<T>(json);
            CheckMessageStateAreEqual(message, deserializedMessage);
            return deserializedMessage;
        }

        protected T CheckMessageStateAreEqual(T expected, T result)
        {
            CheckMessageStateValues(result, expected.MessageId, expected.CorrelationId, expected.UserId, expected.DateTime);
            return result;
        }

        protected T CheckMessageStateValues(T message, Guid messageId, Guid correlationId, string userId, DateTimeOffset dateTime)
        {
            message.MessageId.Should().Be(messageId);
            message.UserId.Should().Be(userId);
            message.CorrelationId.Should().Be(correlationId);
            message.DateTime.Should().Be(dateTime);
            return message;
        }
    }

    public class MessageTest : MessageBaseTest<TestMessage>
    {
        [Fact]
        public void EmptyMessageIdShouldThrowException()
            => Invoking(() => new TestMessage("tpo", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void EmptyCorrelationIdShouldThrowException()
             => Invoking(() => new TestMessage("tpo", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                 .Should()
                 .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void UndefinedUserIdShouldThrowException(string userId)
             => Invoking(() => new TestMessage(userId, Guid.NewGuid()))
                 .Should()
                 .Throw<ArgumentNullException>();

        [Theory]
        [InlineData("testUser")]
        [InlineData("a")]
        [InlineData("ekjAfkd/§?klazlkzaklzalkzakl")]
        public void CheckMessageUserIdState(string userId)
        {
            var message = new TestMessage(userId);
            message.UserId.Should().Be(userId);
            message = new TestMessage(userId, Guid.NewGuid());
            message.UserId.Should().Be(userId);
            message = new TestMessage(userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
            message.UserId.Should().Be(userId);
        }

        [Fact]
        public void CheckCorrelationIdState()
        {
            const string userId = "titi";
            var correlationId = Guid.NewGuid();
            var message = new TestMessage(userId, correlationId);
            message.CorrelationId.Should().Be(correlationId);
            message = new TestMessage(userId, correlationId, Guid.NewGuid(), DateTimeOffset.Now);
            message.CorrelationId.Should().Be(correlationId);
        }

        [Fact]
        public void CheckNewtonSoftJsonSerializeAndDesiarializeMessage()
        {
            var message = new TestMessage("Tu To Ti", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);

            CheckMessageNewtonSoftSerialization(message);
        }

        [Fact]
        public void CheckMessageDateTimeState()
        {
            var dateTime = DateTimeOffset.Now;
            var message = new TestMessage("toto", Guid.NewGuid(), Guid.NewGuid(), dateTime);
            message.DateTime.Should().Be(dateTime);
        }
    }
}
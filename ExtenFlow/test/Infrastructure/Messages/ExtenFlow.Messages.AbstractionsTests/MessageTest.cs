using System;

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

        [JsonConstructor]
        public TestMessage(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public abstract class MessageBaseTest<T> where T : IMessage
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
            CheckMessageStateValues(result, expected.AggregateType, expected.AggregateId, expected.UserId, expected.CorrelationId, expected.MessageId, expected.DateTime);
            return result;
        }

        protected virtual T CheckMessageStateValues(T message, string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
        {
            message.MessageId.Should().Be(messageId);
            message.UserId.Should().Be(userId);
            message.CorrelationId.Should().Be(correlationId);
            message.DateTime.Should().Be(dateTime);
            message.AggregateId.Should().Be(aggregateId);
            message.AggregateType.Should().Be(aggregateType);
            return message;
        }

        protected abstract T Create(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime);

        protected abstract T Create();

        [Theory]
        [InlineData("agType", "agId", "UsrId")]
        [InlineData("agType", "", "UsrId")]
        [InlineData("agType", "      ", "UsrId")]
        [InlineData("agType", null, "UsrId")]
        public void CheckMessageState(string aggregateType, string aggregateId, string userId)
        {
            var messageId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            DateTimeOffset dateTime = DateTimeOffset.Now;
            T message = Create(aggregateType, aggregateId, userId, correlationId, messageId, dateTime);
            CheckMessageStateValues(message, aggregateType, aggregateId, userId, correlationId, messageId, dateTime);
        }

        [Fact]
        public void EmptyMessageIdShouldThrowException()
            => Invoking(() => Create("agType", "AgId", "tpo", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void EmptyCorrelationIdShouldThrowException()
             => Invoking(() => Create("agType", "AgId", "tpo", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                 .Should()
                 .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void UndefinedUserIdShouldThrowException(string userId)
             => Invoking(() => Create("agType", "AgId", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                 .Should()
                 .Throw<ArgumentNullException>();

        [Fact]
        public void CheckNewtonSoftJsonSerializeAndDesiarializeMessage()
            => CheckMessageNewtonSoftSerialization(Create());

        [Fact]
        public void CheckJsonSerializeAndDesiarializeMessage()
            => CheckMessageJsonSerialization(Create());
    }

    public class MessageTest : MessageBaseTest<TestMessage>
    {
        protected override TestMessage Create(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => new TestMessage(aggregateType, aggregateId, userId, correlationId, messageId, dateTime);

        protected override TestMessage Create()
            => new TestMessage("Agtype", "aggreID", "My user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
    }
}
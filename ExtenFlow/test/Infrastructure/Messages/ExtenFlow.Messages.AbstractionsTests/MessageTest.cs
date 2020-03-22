using System;
using System.Collections.Generic;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

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
            result.MessageId.Should().Be(expected.MessageId);
            result.UserId.Should().Be(expected.UserId);
            result.CorrelationId.Should().Be(expected.CorrelationId);
            result.DateTime.Should().Be(expected.DateTime);
            result.AggregateId.Should().Be(expected.AggregateId);
            result.AggregateType.Should().Be(expected.AggregateType);
            return result;
        }

        protected virtual T CheckMessageStateValues(T message, IDictionary<string, object> values)
        {
            message.MessageId.Should().Be((Guid)values[nameof(IMessage.MessageId)]);
            message.UserId.Should().Be((string)values[nameof(IMessage.UserId)]);
            message.CorrelationId.Should().Be((Guid)values[nameof(IMessage.CorrelationId)]);
            message.DateTime.Should().Be((DateTimeOffset)values[nameof(IMessage.DateTime)]);
            message.AggregateId.Should().Be((string)values[nameof(IMessage.AggregateId)]);
            message.AggregateType.Should().Be((string)values[nameof(IMessage.AggregateType)]);
            return message;
        }

        protected abstract IEnumerable<T> Create(IDictionary<string, object> values);

        protected IEnumerable<T> Create()
            => Create(GetTestValues());

        protected virtual Dictionary<string, object> GetTestValues()
        {
            return new Dictionary<string, object> {
                { nameof(IMessage.AggregateType), "agType" },
                { nameof(IMessage.AggregateId), "aggreg ID" },
                { nameof(IMessage.UserId), "My user" },
                { nameof(IMessage.CorrelationId), Guid.NewGuid() },
                { nameof(IMessage.MessageId), Guid.NewGuid() },
                { nameof(IMessage.DateTime), DateTimeOffset.Now }
            };
        }

        [Theory]
        [InlineData("agType", "agId", "UsrId")]
        [InlineData("agType", "", "UsrId")]
        [InlineData("agType", "      ", "UsrId")]
        [InlineData("agType", null, "UsrId")]
        public void CheckMessageState(string aggregateType, string aggregateId, string userId)
        {
            var testValues = new Dictionary<string, object> {
                { nameof(IMessage.AggregateType), aggregateType},
                { nameof(IMessage.AggregateId), aggregateId},
                { nameof(IMessage.UserId), userId },
                { nameof(IMessage.CorrelationId), Guid.NewGuid() },
                { nameof(IMessage.MessageId), Guid.NewGuid() },
                { nameof(IMessage.DateTime), DateTimeOffset.Now } };
            foreach (T message in Create(GetTestValues()))
            {
                CheckMessageStateValues(message, testValues);
            }
        }

        [Fact]
        public void EmptyMessageIdShouldThrowException()
            => Invoking(() =>
                {
                    Dictionary<string, object> values = GetTestValues();
                    values[nameof(IMessage.MessageId)] = Guid.Empty;
                    Create(values);
                })
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void EmptyCorrelationIdShouldThrowException()
            => Invoking(() =>
            {
                Dictionary<string, object> values = GetTestValues();
                values[nameof(IMessage.CorrelationId)] = Guid.Empty;
                Create(values);
            })
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() =>
            {
                Dictionary<string, object> values = GetTestValues();
                values[nameof(IMessage.UserId)] = userId;
                Create(values);
            })
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CheckNewtonSoftJsonSerializeAndDesiarializeMessage()
        {
            foreach (T message in Create())
            {
                CheckMessageNewtonSoftSerialization(message);
            }
        }

        [Fact]
        public void CheckJsonSerializeAndDesiarializeMessage()
        {
            foreach (T message in Create())
            {
                CheckMessageJsonSerialization(message);
            }
        }
    }

    public class MessageTest : MessageBaseTest<TestMessage>
    {
        protected override IEnumerable<TestMessage> Create(IDictionary<string, object> values)
        {
            var message = new TestMessage();
            message.AggregateType = (string)values[nameof(IMessage.AggregateType)];
            message.AggregateId = (string)values[nameof(IMessage.AggregateId)];
            message.UserId = (string)values[nameof(IMessage.UserId)];
            message.CorrelationId = (Guid)values[nameof(IMessage.CorrelationId)];
            message.MessageId = (Guid)values[nameof(IMessage.MessageId)];
            message.DateTime = (DateTimeOffset)values[nameof(IMessage.DateTime)];

            return new TestMessage[]{
                new TestMessage(
                    (string)values[nameof(IMessage.AggregateType)],
                    (string)values[nameof(IMessage.AggregateId)],
                    (string)values[nameof(IMessage.UserId)],
                    (Guid)values[nameof(IMessage.CorrelationId)],
                    (Guid)values[nameof(IMessage.MessageId)],
                    (DateTimeOffset)values[nameof(IMessage.DateTime)]
                ) };
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

using FluentAssertions;

using Newtonsoft.Json;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class MessageFixture<T> where T : IMessage
    {
        public T CheckMessageJsonSerialization(T message)
        {
            string json = System.Text.Json.JsonSerializer.Serialize<T>(message);
            T deserializedMessage = System.Text.Json.JsonSerializer.Deserialize<T>(json);
            CheckMessageStateAreEqual(message, deserializedMessage);
            return deserializedMessage;
        }

        public T CheckMessageNewtonSoftSerialization(T message)
        {
            string json = JsonConvert.SerializeObject(message);
            T deserializedMessage = JsonConvert.DeserializeObject<T>(json);
            CheckMessageStateAreEqual(message, deserializedMessage);
            return deserializedMessage;
        }

        public virtual void CheckMessageState(T result, string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
        {
            result.Id.Should().Be(id);
            result.UserId.Should().Be(userId);
            result.CorrelationId.Should().Be(correlationId);
            result.DateTime.Should().Be(dateTime);
            result.AggregateId.Should().Be(aggregateId);
            result.AggregateType.Should().Be(aggregateType);
        }

        public virtual void CheckMessageStateAreEqual(T expected, T result)
        {
            result.Id.Should().Be(expected.Id);
            result.UserId.Should().Be(expected.UserId);
            result.CorrelationId.Should().Be(expected.CorrelationId);
            result.DateTime.Should().Be(expected.DateTime);
            result.AggregateId.Should().Be(expected.AggregateId);
            result.AggregateType.Should().Be(expected.AggregateType);
        }
    }

    public class MessageTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Type", "Aggr. Id", "User Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. Type", null, "User Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. Type", "", "User Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. Type", "             ", "User Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
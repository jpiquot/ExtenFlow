using System;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class FakeEvent : Event
    {
        [Obsolete]
        public FakeEvent()
        {
        }

        [JsonConstructor]
        public FakeEvent(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public class EventTest : IClassFixture<EventFixture<FakeEvent>>
    {
        private EventFixture<FakeEvent> MessageFixture { get; }

        public EventTest(EventFixture<FakeEvent> messageFixture)
        {
            MessageFixture = messageFixture;
        }

        [Fact]
        public void CreateEvent_EmptyMessageIdShouldThrowException()
            => Invoking(() => new FakeEvent("Aggr. Type", "Aggr. Id", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateEvent_DefaultMessageShouldHaveAMessageId()
            => new FakeEvent().MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateEvent_DefaultMessageShouldHaveACorrelationId()
            => new FakeEvent().CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateEvent_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeEvent("Aggr. Type", "Aggr. Id", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateEvent_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new FakeEvent("Aggr. Type", "Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageNewtonSoftSerialization(new FakeEvent(aggregateType, aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageJsonSerialization(new FakeEvent(aggregateType, aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void CreateEvent_CheckState(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageState(new FakeEvent(aggregateType, aggregateId, userId, correlationId, messageId, dateTime), aggregateType, aggregateId, userId, correlationId, messageId, dateTime);
    }
}
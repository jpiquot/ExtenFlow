using System;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class EventTest : IClassFixture<EventFixture<FakeEvent>>
    {
        public EventTest(EventFixture<FakeEvent> messageFixture)
        {
            MessageFixture = messageFixture;
        }

        private EventFixture<FakeEvent> MessageFixture { get; }

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void CreateEvent_CheckState(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageState(new FakeEvent(aggregateType, aggregateId, userId, correlationId, id, dateTime), aggregateType, aggregateId, userId, correlationId, id, dateTime);

        [Fact]
        public void CreateEvent_DefaultMessageShouldHaveACorrelationId()
            => new FakeEvent().CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateEvent_DefaultMessageShouldHaveAId()
            => new FakeEvent().Id
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateEvent_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeEvent("Aggr. Type", "Aggr. Id", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateEvent_EmptyIdShouldThrowException()
            => Invoking(() => new FakeEvent("Aggr. Type", "Aggr. Id", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
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
        public void DotNetJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageJsonSerialization(new FakeEvent(aggregateType, aggregateId, userId, correlationId, id, dateTime));

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageNewtonSoftSerialization(new FakeEvent(aggregateType, aggregateId, userId, correlationId, id, dateTime));
    }

    public class FakeEvent : Event
    {
        [Obsolete]
        public FakeEvent()
        {
        }

        [JsonConstructor]
        public FakeEvent(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
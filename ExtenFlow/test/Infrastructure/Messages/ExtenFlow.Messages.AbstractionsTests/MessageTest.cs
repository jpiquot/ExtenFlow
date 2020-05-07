using System;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class FakeMessage : Message
    {
        [Obsolete]
        public FakeMessage()
        {
        }

        [JsonConstructor]
        public FakeMessage(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime) : base(aggregateType, aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }

    public class MessageTest : IClassFixture<MessageFixture<FakeMessage>>
    {
        public MessageTest(MessageFixture<FakeMessage> messageFixture)
        {
            MessageFixture = messageFixture;
        }

        private MessageFixture<FakeMessage> MessageFixture { get; }

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void CreateMessage_CheckState(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageState(new FakeMessage(aggregateType, aggregateId, userId, correlationId, id, dateTime), aggregateType, aggregateId, userId, correlationId, id, dateTime);

        [Fact]
        public void CreateMessage_DefaultMessageShouldHaveACorrelationId()
            => new FakeMessage().CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateMessage_DefaultMessageShouldHaveAId()
            => new FakeMessage().Id
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateMessage_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeMessage("Aggr. Type", "Aggr. Id", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateMessage_EmptyIdShouldThrowException()
            => Invoking(() => new FakeMessage("Aggr. Type", "Aggr. Id", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateMessage_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new FakeMessage("Aggr. Type", "Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageJsonSerialization(new FakeMessage(aggregateType, aggregateId, userId, correlationId, id, dateTime));

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageNewtonSoftSerialization(new FakeMessage(aggregateType, aggregateId, userId, correlationId, id, dateTime));
    }
}
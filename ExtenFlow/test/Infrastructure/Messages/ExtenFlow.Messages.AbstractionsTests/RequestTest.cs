using System;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class FakeRequest : Request
    {
        [Obsolete]
        public FakeRequest()
        {
        }

        [JsonConstructor]
        public FakeRequest(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }

    public class RequestTest : IClassFixture<RequestFixture<FakeRequest>>
    {
        public RequestTest(RequestFixture<FakeRequest> messageFixture)
        {
            MessageFixture = messageFixture;
        }

        private RequestFixture<FakeRequest> MessageFixture { get; }

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void CreateRequest_CheckState(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageState(new FakeRequest(aggregateType, aggregateId, userId, correlationId, id, dateTime), aggregateType, aggregateId, userId, correlationId, id, dateTime);

        [Fact]
        public void CreateRequest_DefaultMessageShouldHaveACorrelationId()
            => new FakeRequest().CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRequest_DefaultMessageShouldHaveAId()
            => new FakeRequest().Id
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRequest_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeRequest("Aggr. Type", "Aggr. Id", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateRequest_EmptyIdShouldThrowException()
            => Invoking(() => new FakeRequest("Aggr. Type", "Aggr. Id", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateRequest_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new FakeRequest("Aggr. Type", "Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageJsonSerialization(new FakeRequest(aggregateType, aggregateId, userId, correlationId, id, dateTime));

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => MessageFixture.CheckMessageNewtonSoftSerialization(new FakeRequest(aggregateType, aggregateId, userId, correlationId, id, dateTime));
    }
}
using System;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class FakeQuery : Query<string>
    {
        [Obsolete]
        public FakeQuery()
        {
        }

        [JsonConstructor]
        public FakeQuery(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public class QueryTest : IClassFixture<QueryFixture<string, FakeQuery>>
    {
        private QueryFixture<string, FakeQuery> QueryFixture { get; }

        public QueryTest(QueryFixture<string, FakeQuery> queryFixture)
        {
            QueryFixture = queryFixture;
        }

        [Fact]
        public void CreateQuery_EmptyMessageIdShouldThrowException()
            => Invoking(() => new FakeQuery("Aggr. Type", "Aggr. Id", "User Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateQuery_DefaultMessageShouldHaveAMessageId()
            => new FakeQuery().MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateQuery_DefaultMessageShouldHaveACorrelationId()
            => new FakeQuery().CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateQuery_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeQuery("Aggr. Type", "Aggr. Id", "User Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateQuery_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new FakeQuery("Aggr. Type", "Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => QueryFixture.CheckMessageNewtonSoftSerialization(new FakeQuery(aggregateType, aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => QueryFixture.CheckMessageJsonSerialization(new FakeQuery(aggregateType, aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void CreateQuery_CheckState(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => QueryFixture.CheckMessageState(new FakeQuery(aggregateType, aggregateId, userId, correlationId, messageId, dateTime), aggregateType, aggregateId, userId, correlationId, messageId, dateTime);
    }
}
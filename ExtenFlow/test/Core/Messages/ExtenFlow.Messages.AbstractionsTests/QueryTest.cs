using System;
using ExtenFlow.Messages;
using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Domain.AbstractionsTests
{
    public class FakeQuery : Query<string>
    {
        [Obsolete]
        public FakeQuery()
        {
        }

        [JsonConstructor]
        public FakeQuery(string aggregateType, string aggregateId, string userId, string correlationId = null, string id = null, DateTimeOffset? dateTime = null)
            : base(aggregateType, aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }

    public class QueryTest : IClassFixture<QueryFixture<string, FakeQuery>>
    {
        public QueryTest(QueryFixture<string, FakeQuery> queryFixture)
        {
            QueryFixture = queryFixture;
        }

        private QueryFixture<string, FakeQuery> QueryFixture { get; }

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void CreateQuery_CheckState(string aggregateType, string aggregateId, string userId, string correlationId, string id, DateTimeOffset dateTime)
            => QueryFixture.CheckMessageState(new FakeQuery(aggregateType, aggregateId, userId, correlationId, id, dateTime), aggregateType, aggregateId, userId, correlationId, id, dateTime);

        [Fact]
        public void CreateQuery_DefaultMessageShouldHaveACorrelationId()
            => new FakeQuery().CorrelationId
                .Should()
                .NotBe(string.Empty);

        [Fact]
        public void CreateQuery_DefaultMessageShouldHaveAId()
            => new FakeQuery().Id
                .Should()
                .NotBe(string.Empty);

        [Fact]
        public void CreateQuery_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeQuery("Aggr. Type", "Aggr. Id", "User Id"))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateQuery_EmptyIdShouldThrowException()
            => Invoking(() => new FakeQuery("Aggr. Type", "Aggr. Id", "User Id"))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateQuery_UndefinedUserIdShouldThrowException(string userId)
            => Invoking(() => new FakeQuery("Aggr. Type", "Aggr. Id", userId))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, string correlationId, string id, DateTimeOffset dateTime)
            => QueryFixture.CheckMessageJsonSerialization(new FakeQuery(aggregateType, aggregateId, userId, correlationId, id, dateTime));

        [Theory]
        [ClassData(typeof(MessageTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateType, string aggregateId, string userId, string correlationId, string id, DateTimeOffset dateTime)
            => QueryFixture.CheckMessageNewtonSoftSerialization(new FakeQuery(aggregateType, aggregateId, userId, correlationId, id, dateTime));
    }
}
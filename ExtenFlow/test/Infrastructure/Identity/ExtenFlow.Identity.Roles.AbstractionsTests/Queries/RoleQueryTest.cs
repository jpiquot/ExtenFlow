using System;

using ExtenFlow.Identity.Roles.Queries;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class FakeRoleQuery : RoleQuery<string>
    {
        [Obsolete]
        public FakeRoleQuery()
        {
        }

        [JsonConstructor]
        public FakeRoleQuery(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public class RoleQueryTest : IClassFixture<RoleQueryFixture<string, FakeRoleQuery>>
    {
        public RoleQueryTest(RoleQueryFixture<string, FakeRoleQuery> userQueryFixture)
        {
            RoleQueryFixture = userQueryFixture;
        }

        private RoleQueryFixture<string, FakeRoleQuery> RoleQueryFixture { get; }

        [Theory]
        [ClassData(typeof(RoleQueryTestData))]
        public void CreateRoleQuery_CheckState(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RoleQueryFixture.CheckMessageState(new FakeRoleQuery(aggregateId, userId, correlationId, messageId, dateTime), "Role", aggregateId, userId, correlationId, messageId, dateTime);

        [Fact]
        public void CreateRoleQuery_DefaultMessageShouldHaveACorrelationId()
            => new FakeRoleQuery().CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRoleQuery_DefaultMessageShouldHaveAMessageId()
            => new FakeRoleQuery().MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRoleQuery_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeRoleQuery("Aggr. Id", "Role Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateRoleQuery_EmptyMessageIdShouldThrowException()
            => Invoking(() => new FakeRoleQuery("Aggr. Id", "Role Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateRoleQuery_UndefinedRoleIdShouldThrowException(string userId)
            => Invoking(() => new FakeRoleQuery("Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(RoleQueryTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RoleQueryFixture.CheckMessageJsonSerialization(new FakeRoleQuery(aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(RoleQueryTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RoleQueryFixture.CheckMessageNewtonSoftSerialization(new FakeRoleQuery(aggregateId, userId, correlationId, messageId, dateTime));
    }
}
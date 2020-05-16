using System;

using ExtenFlow.Identity.Roles.Commands;

using FluentAssertions;

using Newtonsoft.Json;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Domain.AbstractionsTests
{
    public class FakeRoleCommand : RoleCommand
    {
        [Obsolete]
        public FakeRoleCommand()
        {
        }

        [JsonConstructor]
        public FakeRoleCommand(string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            : base(aggregateId, null, userId, correlationId, id, dateTime)
        {
        }
    }

    public class RoleCommandTest : IClassFixture<RoleCommandFixture<FakeRoleCommand>>
    {
        public RoleCommandTest(RoleCommandFixture<FakeRoleCommand> userCommandFixture)
        {
            RoleCommandFixture = userCommandFixture;
        }

        private RoleCommandFixture<FakeRoleCommand> RoleCommandFixture { get; }

        [Theory]
        [ClassData(typeof(RoleCommandTestData))]
        public void CreateRoleCommand_CheckState(string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => RoleCommandFixture.CheckMessageState(new FakeRoleCommand(aggregateId, userId, correlationId, id, dateTime), "Role", aggregateId, userId, correlationId, id, dateTime);

        [Fact]
        public void CreateRoleCommand_DefaultMessageShouldHaveACorrelationId()
            => new FakeRoleCommand().CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRoleCommand_DefaultMessageShouldHaveAId()
            => new FakeRoleCommand().Id
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRoleCommand_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new FakeRoleCommand("Aggr. Id", "Role Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateRoleCommand_EmptyIdShouldThrowException()
            => Invoking(() => new FakeRoleCommand("Aggr. Id", "Role Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateRoleCommand_UndefinedRoleIdShouldThrowException(string userId)
            => Invoking(() => new FakeRoleCommand("Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(RoleCommandTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => RoleCommandFixture.CheckMessageJsonSerialization(new FakeRoleCommand(aggregateId, userId, correlationId, id, dateTime));

        [Theory]
        [ClassData(typeof(RoleCommandTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => RoleCommandFixture.CheckMessageNewtonSoftSerialization(new FakeRoleCommand(aggregateId, userId, correlationId, id, dateTime));
    }
}
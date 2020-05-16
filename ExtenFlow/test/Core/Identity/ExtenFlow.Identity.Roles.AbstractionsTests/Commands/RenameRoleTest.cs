using System;
using System.Collections;
using System.Collections.Generic;

using ExtenFlow.Identity.Roles.Commands;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Domain.AbstractionsTests
{
    public class RenameRoleTest : IClassFixture<RoleCommandFixture<RenameRole>>
    {
        public RenameRoleTest(RoleCommandFixture<RenameRole> registerNewRoleFixture)
        {
            RenameRoleFixture = registerNewRoleFixture;
        }

        private RoleCommandFixture<RenameRole> RenameRoleFixture { get; }

        [Theory]
        [ClassData(typeof(RenameRoleTestDate))]
        public void CreateRenameRole_CheckState(string aggregateId, string name, string normalizedName, string concurrencyStamp, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => RenameRoleFixture.CheckMessageState(new RenameRole(aggregateId, name, normalizedName, concurrencyStamp, userId, correlationId, id, dateTime), "Role", aggregateId, userId, correlationId, id, dateTime);

        [Fact]
        public void CreateRenameRole_DefaultMessageShouldHaveACorrelationId()
            => new RenameRole("aggr id", "name", "normalizedName", "OCC1255", "user").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRenameRole_DefaultMessageShouldHaveAId()
            => new RenameRole("aggr id", "name", "normalizedName", "OCC1255", "user").Id
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRenameRole_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new RenameRole("Aggr. Id", "name", "normalized name", "OCC1255", "user", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateRenameRole_EmptyIdShouldThrowException()
            => Invoking(() => new RenameRole("Aggr. Id", "name", "normalized name", "OCC1255", "user", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateRenameRole_UndefinedRoleIdShouldThrowException(string userId)
            => Invoking(() => new RenameRole("Aggr. Id", "name", "normalized name", "OCC1255", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(RenameRoleTestDate))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string name, string normalizedName, string concurrencyStamp, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => RenameRoleFixture.CheckMessageJsonSerialization(new RenameRole(aggregateId, name, normalizedName, concurrencyStamp, userId, correlationId, id, dateTime));

        [Theory]
        [ClassData(typeof(RenameRoleTestDate))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string name, string normalizedName, string concurrencyStamp, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => RenameRoleFixture.CheckMessageNewtonSoftSerialization(new RenameRole(aggregateId, name, normalizedName, concurrencyStamp, userId, correlationId, id, dateTime));
    }

    public class RenameRoleTestDate : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "name @ 123", "normalized name", "OCC1255", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. ID", "name123/%$", "USER", "OCC1255", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
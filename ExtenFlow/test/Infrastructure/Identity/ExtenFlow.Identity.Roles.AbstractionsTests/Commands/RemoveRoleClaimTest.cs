using System;
using System.Collections;
using System.Collections.Generic;

using ExtenFlow.Identity.Roles.Commands;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class RemoveRoleClaimTest : IClassFixture<RoleCommandFixture<RemoveRoleClaim>>
    {
        public RemoveRoleClaimTest(RoleCommandFixture<RemoveRoleClaim> registerNewRoleFixture)
        {
            RemoveRoleClaimFixture = registerNewRoleFixture;
        }

        private RoleCommandFixture<RemoveRoleClaim> RemoveRoleClaimFixture { get; }

        [Theory]
        [ClassData(typeof(RemoveRoleClaimTestDate))]
        public void CreateRemoveRoleClaim_CheckState(string aggregateId, string claimType, string claimValue, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => RemoveRoleClaimFixture.CheckMessageState(new RemoveRoleClaim(aggregateId, claimType, claimValue, userId, correlationId, id, dateTime), "Role", aggregateId, userId, correlationId, id, dateTime);

        [Fact]
        public void CreateRemoveRoleClaim_DefaultMessageShouldHaveACorrelationId()
            => new RemoveRoleClaim("aggr id", "claim type", "claim value", "user").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRemoveRoleClaim_DefaultMessageShouldHaveAId()
            => new RemoveRoleClaim("aggr id", "claim type", "claim value", "user").Id
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRemoveRoleClaim_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new RemoveRoleClaim("Aggr. Id", "claim type", "claim value", "user", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateRemoveRoleClaim_EmptyIdShouldThrowException()
            => Invoking(() => new RemoveRoleClaim("Aggr. Id", "claim type", "claim value", "user", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateRemoveRoleClaim_UndefinedRoleIdShouldThrowException(string userId)
            => Invoking(() => new RemoveRoleClaim("Aggr. Id", "claim type", "claim value", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(RemoveRoleClaimTestDate))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string claimType, string claimValue, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => RemoveRoleClaimFixture.CheckMessageJsonSerialization(new RemoveRoleClaim(aggregateId, claimType, claimValue, userId, correlationId, id, dateTime));

        [Theory]
        [ClassData(typeof(RemoveRoleClaimTestDate))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string claimType, string claimValue, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => RemoveRoleClaimFixture.CheckMessageNewtonSoftSerialization(new RemoveRoleClaim(aggregateId, claimType, claimValue, userId, correlationId, id, dateTime));
    }

    public class RemoveRoleClaimTestDate : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "claimType @ 123", "claim value", "user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. ID", "claimType123/%$", "Claim Value @5298", "USER", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
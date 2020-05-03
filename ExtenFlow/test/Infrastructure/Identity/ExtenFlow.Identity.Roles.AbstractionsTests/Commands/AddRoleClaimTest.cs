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
    public class AddRoleClaimTest : IClassFixture<RoleCommandFixture<AddRoleClaim>>
    {
        public AddRoleClaimTest(RoleCommandFixture<AddRoleClaim> registerNewRoleFixture)
        {
            AddRoleClaimFixture = registerNewRoleFixture;
        }

        private RoleCommandFixture<AddRoleClaim> AddRoleClaimFixture { get; }

        [Theory]
        [ClassData(typeof(AddRoleClaimTestDate))]
        public void CreateAddRoleClaim_CheckState(string aggregateId, string claimType, string claimValue, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => AddRoleClaimFixture.CheckMessageState(new AddRoleClaim(aggregateId, claimType, claimValue, userId, correlationId, messageId, dateTime), "Role", aggregateId, userId, correlationId, messageId, dateTime);

        [Fact]
        public void CreateAddRoleClaim_DefaultMessageShouldHaveACorrelationId()
            => new AddRoleClaim("aggr id", "claim type", "claim value", "user").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateAddRoleClaim_DefaultMessageShouldHaveAMessageId()
            => new AddRoleClaim("aggr id", "claim type", "claim value", "user").MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateAddRoleClaim_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new AddRoleClaim("Aggr. Id", "claim type", "claim value", "user", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateAddRoleClaim_EmptyMessageIdShouldThrowException()
            => Invoking(() => new AddRoleClaim("Aggr. Id", "claim type", "claim value", "user", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateAddRoleClaim_UndefinedRoleIdShouldThrowException(string userId)
            => Invoking(() => new AddRoleClaim("Aggr. Id", "claim type", "claim value", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(AddRoleClaimTestDate))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string claimType, string claimValue, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => AddRoleClaimFixture.CheckMessageJsonSerialization(new AddRoleClaim(aggregateId, claimType, claimValue, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(AddRoleClaimTestDate))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string claimType, string claimValue, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => AddRoleClaimFixture.CheckMessageNewtonSoftSerialization(new AddRoleClaim(aggregateId, claimType, claimValue, userId, correlationId, messageId, dateTime));
    }

    public class AddRoleClaimTestDate : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "claimType @ 123", "claim value", "user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. ID", "claimType123/%$", "Claim Value @5298", "USER", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
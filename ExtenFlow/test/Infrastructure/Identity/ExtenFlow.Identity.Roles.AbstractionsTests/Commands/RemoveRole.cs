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
    public class RemoveRoleTest : IClassFixture<RoleCommandFixture<RemoveRole>>
    {
        public RemoveRoleTest(RoleCommandFixture<RemoveRole> registerNewRoleFixture)
        {
            RemoveRoleFixture = registerNewRoleFixture;
        }

        private RoleCommandFixture<RemoveRole> RemoveRoleFixture { get; }

        [Theory]
        [ClassData(typeof(RemoveRoleTestDate))]
        public void CreateRemoveRole_CheckState(string aggregateId, string concurrencyStamp, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RemoveRoleFixture.CheckMessageState(new RemoveRole(aggregateId, concurrencyStamp, userId, correlationId, messageId, dateTime), "Role", aggregateId, userId, correlationId, messageId, dateTime);

        [Fact]
        public void CreateRemoveRole_DefaultMessageShouldHaveACorrelationId()
            => new RemoveRole("aggr id", "126842396", "user").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRemoveRole_DefaultMessageShouldHaveAMessageId()
            => new RemoveRole("aggr id", "3256535385", "user").MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRemoveRole_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new RemoveRole("Aggr. Id", "6585351232", "user", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateRemoveRole_EmptyMessageIdShouldThrowException()
            => Invoking(() => new RemoveRole("Aggr. Id", "6585351232", "user", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateRemoveRole_UndefinedRoleIdShouldThrowException(string userId)
            => Invoking(() => new RemoveRole("Aggr. Id", "6585351232", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(RemoveRoleTestDate))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string concurrencyStamp, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RemoveRoleFixture.CheckMessageJsonSerialization(new RemoveRole(aggregateId, concurrencyStamp, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(RemoveRoleTestDate))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string concurrencyStamp, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RemoveRoleFixture.CheckMessageNewtonSoftSerialization(new RemoveRole(aggregateId, concurrencyStamp, userId, correlationId, messageId, dateTime));
    }

    public class RemoveRoleTestDate : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "name @ 123", "normalized name", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. ID", "name123/%$", "USER", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
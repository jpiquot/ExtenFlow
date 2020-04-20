using System;
using System.Collections;
using System.Collections.Generic;

using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Queries;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class GetAuthenticatedRoleTest : IClassFixture<RoleQueryFixture<Role, GetAuthenticatedRole>>
    {
        public GetAuthenticatedRoleTest(RoleQueryFixture<Role, GetAuthenticatedRole> getAuthenticatedRoleFixture)
        {
            GetAuthenticatedRoleFixture = getAuthenticatedRoleFixture;
        }

        private RoleQueryFixture<Role, GetAuthenticatedRole> GetAuthenticatedRoleFixture { get; }

        [Theory]
        [ClassData(typeof(GetRoleTestData))]
        public void CreateGetAuthenticatedRole_CheckState(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetAuthenticatedRoleFixture.CheckMessageState(new GetAuthenticatedRole(aggregateId, password, userId, correlationId, messageId, dateTime), "Role", aggregateId, userId, correlationId, messageId, dateTime);

        [Fact]
        public void CreateGetAuthenticatedRole_DefaultMessageShouldHaveACorrelationId()
            => new GetAuthenticatedRole("aggr id", "pass").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateGetAuthenticatedRole_DefaultMessageShouldHaveAMessageId()
            => new GetAuthenticatedRole("aggr id", "pass").MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateGetAuthenticatedRole_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new GetAuthenticatedRole("Aggr. Id", "pass", "Role Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateGetAuthenticatedRole_EmptyMessageIdShouldThrowException()
            => Invoking(() => new GetAuthenticatedRole("Aggr. Id", "pass", "Role Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateGetAuthenticatedRole_UndefinedRoleIdShouldThrowException(string userId)
            => Invoking(() => new GetAuthenticatedRole("Aggr. Id", "pass", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(GetRoleTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetAuthenticatedRoleFixture.CheckMessageJsonSerialization(new GetAuthenticatedRole(aggregateId, password, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(GetRoleTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetAuthenticatedRoleFixture.CheckMessageNewtonSoftSerialization(new GetAuthenticatedRole(aggregateId, password, userId, correlationId, messageId, dateTime));
    }

    public class GetRoleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "passWord @ 123", "Role Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. ID", "passWord123/%$", "USER", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
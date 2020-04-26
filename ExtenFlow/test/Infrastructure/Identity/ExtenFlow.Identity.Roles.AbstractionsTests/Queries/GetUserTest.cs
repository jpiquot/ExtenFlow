using System;

using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Roles.Queries;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class GetRoleTest : IClassFixture<RoleQueryFixture<RoleDetailsModel, GetRoleDetails>>
    {
        public GetRoleTest(RoleQueryFixture<RoleDetailsModel, GetRoleDetails> getRoleDetailsFixture)
        {
            GetRoleDetailsFixture = getRoleDetailsFixture;
        }

        private RoleQueryFixture<RoleDetailsModel, GetRoleDetails> GetRoleDetailsFixture { get; }

        [Theory]
        [ClassData(typeof(RoleQueryTestData))]
        public void CreateGetRole_CheckState(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetRoleDetailsFixture.CheckMessageState(new GetRoleDetails(aggregateId, userId, correlationId, messageId, dateTime), "Role", aggregateId, userId, correlationId, messageId, dateTime);

        [Fact]
        public void CreateGetRole_DefaultMessageShouldHaveACorrelationId()
            => new GetRoleDetails("aggr id", "Role Id").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateGetRole_DefaultMessageShouldHaveAMessageId()
            => new GetRoleDetails("aggr id", "Role Id").MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateGetRole_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new GetRoleDetails("Aggr. Id", "Role Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateGetRole_EmptyMessageIdShouldThrowException()
            => Invoking(() => new GetRoleDetails("Aggr. Id", "Role Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateGetRole_UndefinedRoleIdShouldThrowException(string userId)
            => Invoking(() => new GetRoleDetails("Aggr. Id", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(RoleQueryTestData))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetRoleDetailsFixture.CheckMessageJsonSerialization(new GetRoleDetails(aggregateId, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(RoleQueryTestData))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => GetRoleDetailsFixture.CheckMessageNewtonSoftSerialization(new GetRoleDetails(aggregateId, userId, correlationId, messageId, dateTime));
    }
}
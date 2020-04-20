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
    public class AddNewRoleTest : IClassFixture<RoleCommandFixture<RegisterNewRole>>
    {
        public AddNewRoleTest(RoleCommandFixture<RegisterNewRole> registerNewRoleFixture)
        {
            RegisterNewRoleFixture = registerNewRoleFixture;
        }

        private RoleCommandFixture<RegisterNewRole> RegisterNewRoleFixture { get; }

        [Theory]
        [ClassData(typeof(RegisterNewRoleTestDate))]
        public void CreateRegisterNewRole_CheckState(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RegisterNewRoleFixture.CheckMessageState(new RegisterNewRole(aggregateId, password, userId, correlationId, messageId, dateTime), "Role", aggregateId, userId, correlationId, messageId, dateTime);

        [Fact]
        public void CreateRegisterNewRole_DefaultMessageShouldHaveACorrelationId()
            => new RegisterNewRole("aggr id", "pass").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRegisterNewRole_DefaultMessageShouldHaveAMessageId()
            => new RegisterNewRole("aggr id", "pass").MessageId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateRegisterNewRole_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new RegisterNewRole("Aggr. Id", "pass", "Role Id", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateRegisterNewRole_EmptyMessageIdShouldThrowException()
            => Invoking(() => new RegisterNewRole("Aggr. Id", "pass", "Role Id", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateRegisterNewRole_UndefinedRoleIdShouldThrowException(string userId)
            => Invoking(() => new RegisterNewRole("Aggr. Id", "pass", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(RegisterNewRoleTestDate))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RegisterNewRoleFixture.CheckMessageJsonSerialization(new RegisterNewRole(aggregateId, password, userId, correlationId, messageId, dateTime));

        [Theory]
        [ClassData(typeof(RegisterNewRoleTestDate))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            => RegisterNewRoleFixture.CheckMessageNewtonSoftSerialization(new RegisterNewRole(aggregateId, password, userId, correlationId, messageId, dateTime));
    }

    public class RegisterNewRoleTestDate : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "passWord @ 123", "Role Id", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. ID", "passWord123/%$", "USER", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
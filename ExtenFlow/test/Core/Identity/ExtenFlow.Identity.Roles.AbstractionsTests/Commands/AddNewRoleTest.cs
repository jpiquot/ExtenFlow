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
    public class AddNewRoleTest : IClassFixture<RoleCommandFixture<AddNewRole>>
    {
        public AddNewRoleTest(RoleCommandFixture<AddNewRole> registerNewRoleFixture)
        {
            AddNewRoleFixture = registerNewRoleFixture;
        }

        private RoleCommandFixture<AddNewRole> AddNewRoleFixture { get; }

        [Theory]
        [ClassData(typeof(AddNewRoleTestDate))]
        public void CreateAddNewRole_CheckState(string aggregateId, string name, string normalizedName, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => AddNewRoleFixture.CheckMessageState(new AddNewRole(aggregateId, name, normalizedName, userId, correlationId, id, dateTime), "Role", aggregateId, userId, correlationId, id, dateTime);

        [Fact]
        public void CreateAddNewRole_DefaultMessageShouldHaveACorrelationId()
            => new AddNewRole("aggr id", "name", "normalizedName", "user").CorrelationId
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateAddNewRole_DefaultMessageShouldHaveAId()
            => new AddNewRole("aggr id", "name", "normalizedName", "user").Id
                .Should()
                .NotBe(Guid.Empty);

        [Fact]
        public void CreateAddNewRole_EmptyCorrelationIdShouldThrowException()
            => Invoking(() => new AddNewRole("Aggr. Id", "name", "normalized name", "user", Guid.Empty, Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateAddNewRole_EmptyIdShouldThrowException()
            => Invoking(() => new AddNewRole("Aggr. Id", "name", "normalized name", "user", Guid.NewGuid(), Guid.Empty, DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateAddNewRole_UndefinedRoleIdShouldThrowException(string userId)
            => Invoking(() => new AddNewRole("Aggr. Id", "name", "normalized name", userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now))
                .Should()
                .Throw<ArgumentNullException>();

        [Theory]
        [ClassData(typeof(AddNewRoleTestDate))]
        public void DotNetJsonSerializeMessage_Check(string aggregateId, string name, string normalizedName, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => AddNewRoleFixture.CheckMessageJsonSerialization(new AddNewRole(aggregateId, name, normalizedName, userId, correlationId, id, dateTime));

        [Theory]
        [ClassData(typeof(AddNewRoleTestDate))]
        public void NewtonsoftJsonSerializeMessage_Check(string aggregateId, string name, string normalizedName, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime)
            => AddNewRoleFixture.CheckMessageNewtonSoftSerialization(new AddNewRole(aggregateId, name, normalizedName, userId, correlationId, id, dateTime));
    }

    public class AddNewRoleTestDate : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Aggr. Id", "name @ 123", "normalized name", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
            yield return new object[] { "Aggr. ID", "name123/%$", "USER", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
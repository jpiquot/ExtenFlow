using System;

using ExtenFlow.Identity.Roles.Models;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.Identity.AbstractionsTests
{
    public class RoleTest
    {
        [Fact]
        public void RoleNew_ExpectNoExceptions()
        {
            new Role();
            new Role("my role");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void RoleNewWithEmptyName_ExpectThrowException(string value)
        {
            Invoking(() => new Role(value))
                .Should()
                .Throw<Exception>();
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void RoleSetConcurrencyStamp_ExpectsValue(string value)
        {
            var role = new Role
            {
                ConcurrencyStamp = value
            };
            role.ConcurrencyStamp.Should().Be(value);
        }

        [Fact]
        public void RoleSetId_ExpectsValue()
        {
            var value = "test1";
            var role = new Role
            {
                Id = value
            };
            role.Id.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void RoleSetName_ExpectsValue(string value)
        {
            var role = new Role
            {
                Name = value
            };
            role.Name.Should().Be(value);
            role = new Role(value);
            role.Name.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void RoleSetNormalizedName_ExpectsValue(string value)
        {
            var role = new Role
            {
                NormalizedName = value
            };
            role.NormalizedName.Should().Be(value);
        }
    }
}
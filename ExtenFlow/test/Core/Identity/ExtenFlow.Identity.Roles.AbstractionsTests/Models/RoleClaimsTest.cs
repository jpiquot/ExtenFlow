using System;

using ExtenFlow.Identity.Roles.Models;

using FluentAssertions;

using Xunit;

namespace ExtenFlow.Identity.Roles.AbstractionsTests
{
    public class RoleClaimsTest
    {
        [Fact]
        public void RoleClaimNew_ExpectNoExceptions() => new RoleClaim();

        [Fact]
        public void RoleClaimSetId_ExpectsValue()
        {
            int value = 10;
            var roleClaim = new RoleClaim
            {
                Id = value
            };
            roleClaim.Id.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void RoleClaimSetType_ExpectsValue(string value)
        {
            var roleClaim = new RoleClaim
            {
                ClaimType = value
            };
            roleClaim.ClaimType.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void RoleClaimSetValue_ExpectsValue(string value)
        {
            var roleClaim = new RoleClaim
            {
                ClaimValue = value
            };
            roleClaim.ClaimValue.Should().Be(value);
        }

        [Fact]
        public void RoleSetRoleId_ExpectsValue()
        {
            var value = "testid";
            var role = new RoleClaim
            {
                RoleId = value
            };
            role.RoleId.Should().Be(value);
        }
    }
}
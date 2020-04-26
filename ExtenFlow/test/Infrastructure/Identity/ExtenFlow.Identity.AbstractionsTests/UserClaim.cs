using System;

using ExtenFlow.Identity.Models;

using FluentAssertions;

using Xunit;

namespace ExtenFlow.Identity.AbstractionsTests
{
    public class UserClaimTest
    {
        [Fact]
        public void UserClaimNew_ExpectNoExceptions() => new UserClaim();

        [Fact]
        public void UserClaimSetId_ExpectsValue()
        {
            int value = 10;
            var userClaim = new UserClaim
            {
                Id = value
            };
            userClaim.Id.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserClaimSetType_ExpectsValue(string value)
        {
            var userClaim = new UserClaim
            {
                ClaimType = value
            };
            userClaim.ClaimType.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserClaimSetValue_ExpectsValue(string value)
        {
            var userClaim = new UserClaim
            {
                ClaimValue = value
            };
            userClaim.ClaimValue.Should().Be(value);
        }

        [Fact]
        public void UserSetUserId_ExpectsValue()
        {
            var value = Guid.NewGuid();
            var user = new UserClaim
            {
                UserId = value
            };
            user.UserId.Should().Be(value);
        }
    }
}
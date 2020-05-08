using System;

using ExtenFlow.Identity.Users.Models;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.Identity.AbstractionsTests
{
    public class UserTest
    {
        [Fact]
        public void UserNew_ExpectNoExceptions()
        {
            new User();
            new User("my user");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void UserNewWithEmptyName_ExpectThrowException(string value)
        {
            Invoking(() => new User(value))
                .Should()
                .Throw<Exception>();
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserSetConcurrencyStamp_ExpectsValue(string value)
        {
            var user = new User
            {
                ConcurrencyStamp = value
            };
            user.ConcurrencyStamp.Should().Be(value);
        }

        [Fact]
        public void UserSetId_ExpectsValue()
        {
            var value = "test1";
            var user = new User
            {
                Id = value
            };
            user.Id.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserSetName_ExpectsValue(string value)
        {
            var user = new User
            {
                UserName = value
            };
            user.UserName.Should().Be(value);
            user = new User(value);
            user.UserName.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserSetNormalizedName_ExpectsValue(string value)
        {
            var user = new User
            {
                NormalizedUserName = value
            };
            user.NormalizedUserName.Should().Be(value);
        }
    }
}
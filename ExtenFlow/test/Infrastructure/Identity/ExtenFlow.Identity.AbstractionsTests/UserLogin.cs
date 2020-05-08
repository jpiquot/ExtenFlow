using System;

using ExtenFlow.Identity.Users.Models;

using FluentAssertions;

using Xunit;

namespace ExtenFlow.Identity.AbstractionsTests
{
    public class UserLoginTest
    {
        [Fact]
        public void UserLoginNew_ExpectNoExceptions() => new UserLogin();

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserLoginSetDisplayName_ExpectsValue(string value)
        {
            var userLogin = new UserLogin
            {
                ProviderDisplayName = value
            };
            userLogin.ProviderDisplayName.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserLoginSetKey_ExpectsValue(string value)
        {
            var userLogin = new UserLogin
            {
                ProviderKey = value
            };
            userLogin.ProviderKey.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserLoginSetProvider_ExpectsValue(string value)
        {
            var userLogin = new UserLogin
            {
                LoginProvider = value
            };
            userLogin.LoginProvider.Should().Be(value);
        }

        [Fact]
        public void UserSetUserId_ExpectsValue()
        {
            var value = "test1";
            var user = new UserLogin
            {
                UserId = value
            };
            user.UserId.Should().Be(value);
        }
    }
}
using System;

using ExtenFlow.Identity.Models;

using FluentAssertions;

using Xunit;

namespace ExtenFlow.Identity.AbstractionsTests
{
    public class UserTokenTest
    {
        [Fact]
        public void UserSetUserId_ExpectsValue()
        {
            var value = Guid.NewGuid();
            var user = new UserToken
            {
                UserId = value
            };
            user.UserId.Should().Be(value);
        }

        [Fact]
        public void UserTokenNew_ExpectNoExceptions() => new UserToken();

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserTokenSetLoginProvider_ExpectsValue(string value)
        {
            var userToken = new UserToken
            {
                LoginProvider = value
            };
            userToken.LoginProvider.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserTokenSetName_ExpectsValue(string value)
        {
            var userToken = new UserToken
            {
                Name = value
            };
            userToken.Name.Should().Be(value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("ABC")]
        [InlineData("abc")]
        [InlineData("+-*/")]
        [InlineData(":;,*%")]
        public void UserTokenSetValue_ExpectsValue(string value)
        {
            var userToken = new UserToken
            {
                Value = value
            };
            userToken.Value.Should().Be(value);
        }
    }
}
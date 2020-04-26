using System;

using ExtenFlow.Identity.Models;

using FluentAssertions;

using Xunit;

namespace ExtenFlow.Identity.AbstractionsTests
{
    public class UserRoleTest
    {
        [Fact]
        public void UserRoleNew_ExpectNoExceptions() => new UserRole();

        [Fact]
        public void UserSetRoleId_ExpectsValue()
        {
            var value = Guid.NewGuid();
            var user = new UserRole
            {
                RoleId = value
            };
            user.RoleId.Should().Be(value);
        }

        [Fact]
        public void UserSetUserId_ExpectsValue()
        {
            var value = Guid.NewGuid();
            var user = new UserRole
            {
                UserId = value
            };
            user.UserId.Should().Be(value);
        }
    }
}
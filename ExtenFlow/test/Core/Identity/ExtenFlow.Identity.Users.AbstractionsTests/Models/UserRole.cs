using ExtenFlow.Identity.Users.Models;

using FluentAssertions;

using Xunit;

namespace ExtenFlow.Identity.Users.AbstractionsTests
{
    public class UserRoleTest
    {
        [Fact]
        public void UserRoleNew_ExpectNoExceptions() => new UserRole();

        [Fact]
        public void UserSetRoleId_ExpectsValue()
        {
            var value = "test1";
            var user = new UserRole
            {
                RoleId = value
            };
            user.RoleId.Should().Be(value);
        }
    }
}
using ExtenFlow.Identity.Models;

using FluentAssertions;

using Xunit;

namespace ExtenFlow.Identity.AbstractionsTests
{
    public class RoleTest
    {
        [Fact]
        public void RoleNew_ExpectName()
        {
            var role = new Role("my role");
            role.Name.Should().Be("my role");
            role.NormalizedName.Should().Be("myrole");
        }

        [Fact]
        public void RoleNew_ExpectNoExceptions()
        {
            _ = new Role();
            _ = new Role("my role");
        }
    }
}
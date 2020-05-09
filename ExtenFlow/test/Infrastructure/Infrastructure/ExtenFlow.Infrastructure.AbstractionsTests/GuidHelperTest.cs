using System;

using FluentAssertions;

using Xunit;

namespace ExtenFlow.Infrastructure.AbstractionsTests
{
    public class GuidHelperTest
    {
        [Fact]
        public void DefaultGuidToBase64String_ExpectsEmptyString()
        {
            string id = Guid.Empty.ToBase64String();
            id.Should().Be(string.Empty);
        }

        [Fact]
        public void GuidToBase64String_ExpectsLength22()
        {
            string id = Guid.NewGuid().ToBase64String();
            id.Should().NotBeNullOrWhiteSpace();
            id.Length.Should().Be(22);
        }
    }
}
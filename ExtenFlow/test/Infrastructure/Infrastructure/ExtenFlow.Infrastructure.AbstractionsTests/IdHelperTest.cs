using FluentAssertions;

using Xunit;

#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Infrastructure.AbstractionsTests
{
    public class IdHelperTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateDefaultId10WithNoDuplicateCheck_ExpectsTruncatedGuid(string proposedId)
        {
            string id = proposedId.GenerateId(null, 10);
            id.Should().NotBeNullOrWhiteSpace();
            id.Length.Should().Be(10);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateDefaultId22WithNoDuplicateCheck_ExpectsGuid(string proposedId)
        {
            string id = proposedId.GenerateId(null, 22);
            id.Should().NotBeNullOrWhiteSpace();
            id.Length.Should().Be(22);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("                      ")]
        public void CreateDefaultId50WithNoDuplicateCheck_ExpectsGuid(string proposedId)
        {
            string id = proposedId.GenerateId(null, 50);
            id.Should().NotBeNullOrWhiteSpace();
            id.Length.Should().Be(22);
        }

        [Fact]
        public void CreateIdWithDuplicateCheck_ExpectsIndexedValue()
        {
            string proposedId = "mykey";
            string expectedId = "mykey7";
            string id = proposedId.GenerateId((s) => (s != expectedId), 22);
            id.Should().NotBeNullOrWhiteSpace();
            id.Should().Be(expectedId);
        }

        [Fact]
        public void CreateIdWithDuplicateCheckAllTrue_ExpectsGuidValue()
        {
            string proposedId = "a value";
            string id = proposedId.GenerateId((s) => true, 22);
            id.Should().NotBeNullOrWhiteSpace();
            id.Length.Should().Be(22);
        }

        [Fact]
        public void CreateIdWithLengthMoreThanMaxAndDuplicateCheck_ExpectsTruncatedIndexedValue()
        {
            string proposedId = "mykey";
            string expectedId = "myk7";
            string id = proposedId.GenerateId((s) => (s != expectedId), 4);
            id.Should().NotBeNullOrWhiteSpace();
            id.Should().Be(expectedId);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("abcdefghijklm")]
        [InlineData("abcdefghijklm512369212")]
        public void CreateIdWithSizeLessThanMax_ExpectsSameValue(string proposedId)
        {
            string id = proposedId.GenerateId(null, 22);
            id.Should().NotBeNullOrWhiteSpace();
            id.Should().Be(proposedId);
        }

        [Theory]
        [InlineData("akldazlkckslqlk")]
        [InlineData("abcdefghi;,nkj")]
        [InlineData("abcdefghijklm512369212")]
        public void CreateIdWithSizeMoreThanMax_ExpectsTruncatedValue(string proposedId)
        {
            string id = proposedId.GenerateId(null, 10);
            id.Should().NotBeNullOrWhiteSpace();
            id.Length.Should().Be(10);
            id.Should().Be(proposedId.Substring(0, 10));
        }
    }
}
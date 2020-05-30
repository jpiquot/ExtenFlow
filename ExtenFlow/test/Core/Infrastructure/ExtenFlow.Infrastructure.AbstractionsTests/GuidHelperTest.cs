using System;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData("                      ")]
        public void EmptyStringToGuid_ExpectsException(string value)
            => Invoking(() => value.ToGuid())
                .Should()
                .Throw<ArgumentException>();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData("                      ")]
        public void EmptyStringToGuidOrDefault_ExpectsNull(string value)
            => value.ToGuidOrDefault().Should().BeNull();

        [Fact]
        public void GuidToBase64String_ExpectsLength22()
        {
            string id = Guid.NewGuid().ToBase64String();
            id.Should().NotBeNullOrWhiteSpace();
            id.Length.Should().Be(22);
        }

        [Fact]
        public void GuidToBase64ToGuid_ExpectsSameValue()
        {
            for (int i = 0; i < 100; i++)
            {
                var guid = Guid.NewGuid();
                string id = guid.ToBase64String();
                id.ToGuid().Should().Be(guid);
                (id + "==").ToGuid().Should().Be(guid);
            }
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa555555555555555555555555")]
        [InlineData("²²²²²ùùùùçççèèè")]
        public void InvalidStringToGuid_ExpectsException(string value)
            => Invoking(() => value.ToGuid())
                .Should()
                .Throw<ArgumentException>();

        [Fact]
        public void String22AToGuid_ExpectsEmptyGuid()
            => "AAAAAAAAAAAAAAAAAAAAAA".ToGuid().Should().Be(Guid.Empty);

        [Theory]
        [InlineData("1234567890123456789012")]
        [InlineData("abcdefghijklmnopqrstuv")]
        [InlineData("++++++++++++++++++++++")]
        [InlineData("----------------------")]
        [InlineData("______________________")]
        [InlineData("//////////////////////")]
        [InlineData("A53BABEB423F43C884FE3A")]
        [InlineData("A53BABEB423F43C884FE3A==")]
        public void String22ToGuid_ExpectsGuid(string value)
            => value.ToGuid().Should().NotBe(Guid.Empty);

        [Fact]
        public void StringToGuid_ExpectsSameValue()
        {
            for (int i = 0; i < 100; i++)
            {
                var guid = Guid.NewGuid();
                guid.ToString().ToGuid().Should().Be(guid);
            }
        }
    }
}
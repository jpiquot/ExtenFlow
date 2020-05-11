using FluentAssertions;

using Xunit;

namespace ExtenFlow.Infrastructure.AbstractionsTests
{
    public class ResultTest
    {
        [Fact]
        public void CreateFailedResultWithList_ExpectsFailedAndContainMessages()
        {
            string[] msgs = { "test message 1", "test message 2", "test message 3" };

            var result = Result.Failed(msgs);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(3);
            result.Messages.Should().BeEquivalentTo(msgs);
        }

        [Fact]
        public void CreateFailedResultWithParams_ExpectsFailedAndContainMessages()
        {
            string msg1 = "test message 1";
            string msg2 = "test message 2";
            string msg3 = "test message 3";
            var result = Result.Failed(msg1);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(1);
            result.Messages[0].Should().Be(msg1);
            result = Result.Failed(msg1, msg2, msg3);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(3);
            result.Messages[0].Should().Be(msg1);
        }

        [Fact]
        public void CreateSuccesResultWithList_ExpectsSuccessAndContainMessages()
        {
            string[] msgs = { "test message 1", "test message 2", "test message 3" };

            var result = new Result(false, msgs);
            result.HasFailed.Should().Be(false);
            result.Messages.Should().HaveCount(3);
            result.Messages.Should().BeEquivalentTo(msgs);
        }

        [Fact]
        public void CreateSuccessResult_ExpectsSuccess()
        {
            var result = Result.Succeeded();
            result.HasFailed.Should().Be(false);
            result.Messages.Should().HaveCount(0);
        }

        [Fact]
        public void CreateSuccessResultWithParams_ExpectsSuccessAndContainMessages()
        {
            string msg1 = "test message 1";
            string msg2 = "test message 2";
            string msg3 = "test message 3";
            var result = new Result(false, msg1);
            result.HasFailed.Should().Be(false);
            result.Messages.Should().HaveCount(1);
            result.Messages[0].Should().Be(msg1);
            result = new Result(false, msg1, msg2, msg3);
            result.HasFailed.Should().Be(false);
            result.Messages.Should().HaveCount(3);
            result.Messages[0].Should().Be(msg1);
        }
    }
}
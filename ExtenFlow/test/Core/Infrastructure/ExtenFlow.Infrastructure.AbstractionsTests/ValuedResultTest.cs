using System;

using FluentAssertions;

using Xunit;

using static FluentAssertions.FluentActions;

namespace ExtenFlow.Infrastructure.AbstractionsTests
{
    public class ValuedResultTest
    {
        [Fact]
        public void CreateFailedResultWithList_ExpectsFailedAndContainMessages()
        {
            string[] msgs = { "test message 1", "test message 2", "test message 3" };

            Result result = Result.Failed<Guid>(msgs);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(3);
            result.Messages.Should().BeEquivalentTo(msgs);
            result = Result.Failed<string>(msgs);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(3);
            result.Messages.Should().BeEquivalentTo(msgs);
            result = Result.Failed<int>(msgs);
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
            Result result = Result.Failed<Guid>(msg1);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(1);
            result.Messages[0].Should().Be(msg1);
            result = Result.Failed<Guid>(msg1, msg2, msg3);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(3);
            result.Messages[0].Should().Be(msg1);
            result = Result.Failed<string>(msg1);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(1);
            result.Messages[0].Should().Be(msg1);
            result = Result.Failed<string>(msg1, msg2, msg3);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(3);
            result.Messages[0].Should().Be(msg1);
            result = Result.Failed<int>(msg1);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(1);
            result.Messages[0].Should().Be(msg1);
            result = Result.Failed<int>(msg1, msg2, msg3);
            result.HasFailed.Should().Be(true);
            result.Messages.Should().HaveCount(3);
            result.Messages[0].Should().Be(msg1);
        }

        [Fact]
        public void CreateGuidSuccessResult_ExpectsSuccess()
        {
            var value = Guid.NewGuid();
            var result = Result.Succeeded(value);
            result.HasFailed.Should().Be(false);
            result.Messages.Should().HaveCount(0);
            result.Value.Should().Be(value);
        }

        [Fact]
        public void CreateIntSuccessResult_ExpectsSuccess()
        {
            var result = Result.Succeeded(9955);
            result.HasFailed.Should().Be(false);
            result.Messages.Should().HaveCount(0);
            result.Value.Should().Be(9955);
        }

        [Fact]
        public void CreateNullValuedResult_ExpectsException()
            => Invoking(() => Result.Succeeded<string>(null))
                .Should()
                .Throw<ArgumentNullException>();

        [Fact]
        public void CreateStringSuccessResult_ExpectsSuccess()
        {
            var result = Result.Succeeded("test");
            result.HasFailed.Should().Be(false);
            result.Messages.Should().HaveCount(0);
            result.Value.Should().Be("test");
        }

        [Fact]
        public void CreateSuccesResultWithList_ExpectsSuccessAndContainMessages()
        {
            string[] msgs = { "test message 1", "test message 2", "test message 3" };

            var result = new Result<string>("test", false, msgs);
            result.HasFailed.Should().Be(false);
            result.Messages.Should().HaveCount(3);
            result.Messages.Should().BeEquivalentTo(msgs);
            result.Value.Should().Be("test");
            var resultInt = new Result<int>(10, false, msgs);
            resultInt.HasFailed.Should().Be(false);
            resultInt.Messages.Should().HaveCount(3);
            resultInt.Messages.Should().BeEquivalentTo(msgs);
            resultInt.Value.Should().Be(10);
        }

        [Fact]
        public void GetFailedResultValue_ExpectsException()
            => Invoking(() => Result.Failed<string>("test msg").Value)
                .Should()
                .Throw<InvalidOperationException>();
    }
}
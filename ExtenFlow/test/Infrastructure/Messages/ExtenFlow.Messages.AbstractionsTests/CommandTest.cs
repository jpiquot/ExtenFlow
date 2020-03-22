using System;

using Newtonsoft.Json;

namespace ExtenFlow.Messages.AbstractionsTests
{
    public class TestCommand : Command
    {
        [Obsolete]
        public TestCommand()
        {
        }

        [JsonConstructor]
        public TestCommand(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }

    public abstract class CommandBaseTest<T> : RequestBaseTest<T> where T : ICommand
    {
    }

    public class CommandTest : CommandBaseTest<TestCommand>
    {
        protected override TestCommand Create(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
           => new TestCommand(aggregateType, aggregateId, userId, correlationId, messageId, dateTime);

        protected override TestCommand Create()
            => new TestCommand("Agtype", "aggreID", "My user", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now);
    }
}
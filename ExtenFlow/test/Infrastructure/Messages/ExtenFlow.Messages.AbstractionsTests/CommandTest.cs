using System;
using System.Collections.Generic;

using Newtonsoft.Json;

#pragma warning disable CS0612 // Type or member is obsolete

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
        protected override IEnumerable<TestCommand> Create(IDictionary<string, object> values)
        {
            var message = new TestCommand();
            message.AggregateType = (string)values[nameof(IMessage.AggregateType)];
            message.AggregateId = (string)values[nameof(IMessage.AggregateId)];
            message.UserId = (string)values[nameof(IMessage.UserId)];
            message.CorrelationId = (Guid)values[nameof(IMessage.CorrelationId)];
            message.MessageId = (Guid)values[nameof(IMessage.MessageId)];
            message.DateTime = (DateTimeOffset)values[nameof(IMessage.DateTime)];

            return new TestCommand[]{message,
                new TestCommand(
                    (string)values[nameof(IMessage.AggregateType)],
                    (string)values[nameof(IMessage.AggregateId)],
                    (string)values[nameof(IMessage.UserId)],
                    (Guid)values[nameof(IMessage.CorrelationId)],
                    (Guid)values[nameof(IMessage.MessageId)],
                    (DateTimeOffset)values[nameof(IMessage.DateTime)]
                ) };
        }
    }
}
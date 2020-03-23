using System;

using ExtenFlow.Messages;
using ExtenFlow.Security.Users.Aggregates;

namespace ExtenFlow.Security.Users.Commands
{
    public abstract class UserCommand : Command
    {
        [Obsolete("Can only be used by serializers", true)]
        protected UserCommand()
        {
            AggregateType = UserAggregateDefinition.Name;
        }

        protected UserCommand(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(UserAggregateDefinition.Name, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
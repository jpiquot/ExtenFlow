using System;

using ExtenFlow.Messages;
using ExtenFlow.Identity.Users.Aggregates;

namespace ExtenFlow.Identity.Users.Commands
{
    /// <summary>
    /// A command for the user aggregate
    /// </summary>
    public abstract class UserCommand : Command
    {
        /// <summary>
        /// User command constructor
        /// </summary>
        /// <remarks>Do not use. Defined for serializers.</remarks>
        [Obsolete("Can only be used by serializers", true)]
        protected UserCommand()
        {
            AggregateType = UserAggregateDefinition.Name;
        }

        /// <summary>
        /// A command for the user aggregate constructor
        /// </summary>
        /// <param name="aggregateId">The id of the user</param>
        /// <param name="userId">The id of the user submitting this command</param>
        /// <param name="correlationId">The correlation id</param>
        /// <param name="messageId">The id of this command</param>
        /// <param name="dateTime">The created date and time of the command</param>
        protected UserCommand(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(UserAggregateDefinition.Name, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
using System;

using ExtenFlow.Identity.Users.Aggregates;
using ExtenFlow.Messages;

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
        /// Initializes a new instance of the <see cref="UserCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id</param>
        /// <param name="concurrencyStamp">Concurrency stamp used for optimistic concurrency check.</param>
        /// <param name="userId">The user submitting the command</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events
        /// </param>
        /// <param name="messageId">The Id of this command</param>
        /// <param name="dateTime">The date time of the command submission</param>
        protected UserCommand(string aggregateId, string? concurrencyStamp, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(UserAggregateDefinition.Name, aggregateId, concurrencyStamp, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
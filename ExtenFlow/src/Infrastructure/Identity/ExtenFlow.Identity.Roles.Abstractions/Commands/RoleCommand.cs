using System;

using ExtenFlow.Identity.Models;
using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Roles
{
    /// <summary>
    /// Base Role command class
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.Command"/>
    public abstract class RoleCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleCommand"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected RoleCommand()
        {
            AggregateType = nameof(Role);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="concurrencyStamp">Concurrency stamp used for optimistic concurrency check.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="messageId">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        protected RoleCommand(string aggregateId, string? concurrencyStamp, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(nameof(Role), aggregateId, concurrencyStamp, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
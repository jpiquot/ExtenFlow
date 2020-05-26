using System;

using ExtenFlow.Domain;
using ExtenFlow.Identity.Roles.Models;

namespace ExtenFlow.Identity.Roles.Commands
{
    /// <summary>
    /// Base Role command class
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.Command"/>
    public abstract class RoleCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleCommand"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected RoleCommand()
        {
            AggregateType = nameof(Role);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="concurrencyCheckStamp">
        /// Concurrency stamp used for optimistic concurrency check.
        /// </param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        protected RoleCommand(string aggregateId,
                              string userId,
                              string? concurrencyCheckStamp = null,
                              string? correlationId = null,
                              string? id = null,
                              DateTimeOffset? dateTime = null)
            : base(DefaultAggregateType, aggregateId, userId, concurrencyCheckStamp, correlationId, id, dateTime)
        {
        }

        /// <summary>
        /// Default aggregate type
        /// </summary>
        /// <value>The default type of the aggregate.</value>
        public static string DefaultAggregateType => nameof(Role);
    }
}
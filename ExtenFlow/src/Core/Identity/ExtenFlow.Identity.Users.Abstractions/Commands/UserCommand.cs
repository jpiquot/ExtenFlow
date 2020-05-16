using System;

using ExtenFlow.Identity.Users.Models;
using ExtenFlow.Domain;

namespace ExtenFlow.Identity.Users.Commands
{
    /// <summary>
    /// Base User command class
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.Command"/>
    public abstract class UserCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCommand"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected UserCommand()
        {
            AggregateType = nameof(User);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCommand"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="concurrencyStamp">Concurrency stamp used for optimistic concurrency check.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        protected UserCommand(string aggregateId, string? concurrencyStamp, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime) : base(nameof(User), aggregateId, concurrencyStamp, userId, correlationId, id, dateTime)
        {
        }
    }
}
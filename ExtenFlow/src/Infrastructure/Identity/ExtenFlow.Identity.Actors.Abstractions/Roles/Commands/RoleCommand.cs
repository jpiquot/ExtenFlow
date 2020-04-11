using System;
using ExtenFlow.Identity.Models;
using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Actors
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
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="dateTime">The date time.</param>
        protected RoleCommand(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(nameof(Role), aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
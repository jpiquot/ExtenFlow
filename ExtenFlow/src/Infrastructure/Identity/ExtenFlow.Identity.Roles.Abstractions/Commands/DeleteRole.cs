using System;

namespace ExtenFlow.Identity.Roles
{
    /// <summary>
    /// Delete role command
    /// </summary>
    /// <seealso cref="RoleCommand"/>
    public class DeleteRole : RoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRole"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public DeleteRole()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRole"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="concurrencyStamp">Concurrency stamp used for optimistic concurrency check.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="messageId">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public DeleteRole(string aggregateId, string concurrencyStamp, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, concurrencyStamp, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
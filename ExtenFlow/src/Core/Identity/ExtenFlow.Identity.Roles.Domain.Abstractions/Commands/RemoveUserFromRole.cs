using System;

namespace ExtenFlow.Identity.Roles.Domain.Commands
{
    /// <summary>
    /// Remove the user from the role command
    /// </summary>
    /// <seealso cref="RoleCommand"/>
    public class RemoveUserFromRole : RoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserFromRole"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RemoveUserFromRole()
        {
            RoleUserId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserFromRole"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="roleUserId">The role new roleUserId.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="concurrencyCheckStamp">
        /// Concurrency stamp used for optimistic concurrency check.
        /// </param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RemoveUserFromRole(string aggregateId, string roleUserId, string userId, string concurrencyCheckStamp, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, concurrencyCheckStamp, correlationId, id, dateTime)
        {
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
            if (string.IsNullOrWhiteSpace(roleUserId))
            {
                throw new ArgumentNullException(nameof(roleUserId));
            }
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
            RoleUserId = roleUserId;
        }

        /// <summary>
        /// Gets the user identifier to add to the role.
        /// </summary>
        /// <value>The role user Id.</value>
        public string RoleUserId { get; }
    }
}
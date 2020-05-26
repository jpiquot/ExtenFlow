using System;

namespace ExtenFlow.Identity.Roles.Commands
{
    /// <summary>
    /// Add a user to a role command
    /// </summary>
    /// <seealso cref="RoleCommand"/>
    public class AddUserToRole : RoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserToRole"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public AddUserToRole()
        {
            RoleUserId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserToRole"/> class.
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
        public AddUserToRole(string aggregateId, string roleUserId, string userId, string concurrencyCheckStamp, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
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
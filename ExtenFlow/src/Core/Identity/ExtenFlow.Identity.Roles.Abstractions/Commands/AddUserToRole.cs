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
        /// <param roleUserId="aggregateId">Aggragate Id.</param>
        /// <param roleUserId="roleUserId">The role new roleUserId.</param>
        /// <param roleUserId="userId">The user submitting the command.</param>
        /// <param roleUserId="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param roleUserId="id">The Id of this command.</param>
        /// <param roleUserId="dateTime">The date time of the command submission.</param>
        public AddUserToRole(string aggregateId, string roleUserId, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, null, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
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
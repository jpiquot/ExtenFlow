using System;

namespace ExtenFlow.Identity.Roles.Domain.Events
{
    /// <summary>
    /// User added to role
    /// </summary>
    /// <seealso cref="RoleEvent"/>
    public class UserAddedToRole : RoleEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAddedToRole"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public UserAddedToRole()
        {
            RoleUserId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAddedToRole"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="roleUserId">The role new name.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public UserAddedToRole(string aggregateId, string roleUserId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId, id, dateTime)
        {
            RoleUserId = roleUserId;
        }

        /// <summary>
        /// Gets the new role name.
        /// </summary>
        /// <value>The name.</value>
        public string RoleUserId { get; }
    }
}
using System;

namespace ExtenFlow.Identity.Roles.Application.Queries
{
    /// <summary>
    /// Class IsRoleNameRegistered. Implements the <see cref="RoleQuery{RoleDetailsViewModel}"/>
    /// </summary>
    /// <seealso cref="RoleQuery{RoleDetailsViewModel}"/>
    public class IsRoleNameRegistered : RoleNameRegistryQuery<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsRoleNameRegistered"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public IsRoleNameRegistered()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsRoleNameRegistered"/> class.
        /// </summary>
        /// <param name="aggregateId">The normalized role name.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public IsRoleNameRegistered(string aggregateId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
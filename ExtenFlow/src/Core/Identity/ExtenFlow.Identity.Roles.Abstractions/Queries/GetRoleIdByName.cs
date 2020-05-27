using System;

namespace ExtenFlow.Identity.Roles.Queries
{
    /// <summary>
    /// Class GetRoleIdByName. Implements the <see cref="RoleQuery{RoleDetailsViewModel}"/>
    /// </summary>
    /// <seealso cref="RoleQuery{RoleDetailsViewModel}"/>
    public class GetRoleIdByName : RoleNameRegistryQuery<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleIdByName"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public GetRoleIdByName()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleIdByName"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public GetRoleIdByName(string aggregateId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
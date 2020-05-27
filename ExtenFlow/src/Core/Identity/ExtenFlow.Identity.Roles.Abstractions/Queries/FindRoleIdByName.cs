using System;

namespace ExtenFlow.Identity.Roles.Queries
{
    /// <summary>
    /// Class FindRoleIdByName. Implements the <see cref="ExtenFlow.Identity.Roles.Queries.RoleNameRegistryQuery{T}"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Roles.Queries.RoleNameRegistryQuery{T}"/>
    public class FindRoleIdByName : RoleNameRegistryQuery<string?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindRoleIdByName"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public FindRoleIdByName()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FindRoleIdByName"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public FindRoleIdByName(string aggregateId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
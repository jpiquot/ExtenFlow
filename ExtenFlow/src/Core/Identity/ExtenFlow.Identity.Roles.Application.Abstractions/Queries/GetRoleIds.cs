using System;
using System.Collections.Generic;
using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Roles.Application.Queries
{
    /// <summary>
    /// Class GetRoleIds. Implements the <see cref="RoleQuery{RoleDetailsViewModel}"/>
    /// </summary>
    /// <seealso cref="RoleQuery{RoleDetailsViewModel}"/>
    public class GetRoleIds : Query<IList<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleIds"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public GetRoleIds()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleIds"/> class.
        /// </summary>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public GetRoleIds(string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(AggregateName.RoleCollection, AggregateName.Role, userId, correlationId, id, dateTime)
        {
        }
    }
}
using System;

using ExtenFlow.Domain;

namespace ExtenFlow.Identity.Roles.Events
{
    /// <summary>
    /// Class NormalizedRoleNameUnregistred. Implements the <see cref="ExtenFlow.Identity.Roles.Events.RoleEvent"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Roles.Events.RoleEvent"/>
    public class NormalizedRoleNameDeregistred : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedRoleNameDeregistred"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public NormalizedRoleNameDeregistred()
        {
            RoleId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedRoleNameDeregistred"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="roleId"></param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public NormalizedRoleNameDeregistred(string aggregateId, string roleId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base("NormalizedRoleName", aggregateId, userId, correlationId, id, dateTime)
        {
            RoleId = roleId;
        }

        /// <summary>
        /// Gets the normalized role name.
        /// </summary>
        /// <value>The name.</value>
        public string RoleId { get; }
    }
}
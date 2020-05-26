using System;

using ExtenFlow.Domain;

namespace ExtenFlow.Identity.Roles.Commands
{
    /// <summary>
    /// Class DeregisterNormalizedRoleName. Implements the <see cref="ExtenFlow.Identity.Roles.Commands.RoleCommand"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Roles.Commands.RoleCommand"/>
    public class DeregisterNormalizedRoleName : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeregisterNormalizedRoleName"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public DeregisterNormalizedRoleName()
        {
            RoleId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeregisterNormalizedRoleName"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="roleId"></param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="concurrencyCheckStamp">The optimistic concurrency check stamp.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public DeregisterNormalizedRoleName(string aggregateId, string roleId, string userId, string concurrencyCheckStamp, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base("NormalizedRoleName", aggregateId, userId, concurrencyCheckStamp, correlationId, id, dateTime)
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
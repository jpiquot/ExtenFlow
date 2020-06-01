using System;

namespace ExtenFlow.Identity.Roles.Domain.Events
{
    /// <summary>
    /// Role deleted
    /// </summary>
    /// <seealso cref="RoleEvent"/>
    public class RoleRemoved : RoleEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRemoved"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RoleRemoved()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRemoved"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RoleRemoved(string aggregateId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
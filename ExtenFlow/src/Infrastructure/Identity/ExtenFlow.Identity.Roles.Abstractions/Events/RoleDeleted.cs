using System;

namespace ExtenFlow.Identity.Roles.Events
{
    /// <summary>
    /// Role deleted
    /// </summary>
    /// <seealso cref="RoleEvent"/>
    public class RoleDeleted : RoleEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDeleted"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RoleDeleted()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDeleted"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="messageId">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RoleDeleted(string aggregateId, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
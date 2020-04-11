using System;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// Role deleted
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Actors.RoleEvent"/>
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
        /// <param name="aggregateId">The role id</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="dateTime">The date and time.</param>
        public RoleDeleted(string aggregateId, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
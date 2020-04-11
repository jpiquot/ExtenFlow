using System;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// Delete role command
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Actors.RoleCommand"/>
    public class DeleteRole : RoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRole"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public DeleteRole()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRole"/> class.
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="dateTime">The date time.</param>
        public DeleteRole(string aggregateId, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
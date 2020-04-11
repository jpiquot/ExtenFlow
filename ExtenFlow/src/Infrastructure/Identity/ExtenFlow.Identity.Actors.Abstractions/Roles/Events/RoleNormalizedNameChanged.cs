using System;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// Role normilized name changed
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Actors.RoleEvent"/>
    public class RoleNormilizedNameChanged : RoleEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNormilizedNameChanged"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RoleNormilizedNameChanged()
        {
            NormalizedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNormilizedNameChanged"/> class.
        /// </summary>
        /// <param name="aggregateId">The role id</param>
        /// <param name="normalizedName">The role normalized name.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="dateTime">The date and time.</param>
        public RoleNormilizedNameChanged(string aggregateId, string normalizedName, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            NormalizedName = normalizedName;
        }

        /// <summary>
        /// Gets the new role normalized name.
        /// </summary>
        /// <value>The name of the normalized.</value>
        public string NormalizedName { get; }
    }
}
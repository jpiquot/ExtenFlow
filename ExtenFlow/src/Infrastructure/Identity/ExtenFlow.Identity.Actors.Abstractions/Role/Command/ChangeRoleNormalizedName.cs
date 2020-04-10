using System;

namespace ExtenFlow.Identity.Actors.Role.Command
{
    /// <summary>
    /// Change role normalized name
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Actors.RoleCommand"/>
    public class ChangeRoleNormalizedName : RoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeRoleNormalizedName"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public ChangeRoleNormalizedName()
        {
            NormalizedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeRoleNormalizedName"/> class.
        /// </summary>
        /// <param name="aggregateId">The role identifier.</param>
        /// <param name="normalizedName">Name of the normalized.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="dateTime">The date time.</param>
        public ChangeRoleNormalizedName(string aggregateId, string normalizedName, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
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
using System;

namespace ExtenFlow.Identity.Roles
{
    /// <summary>
    /// Role normilized name changed
    /// </summary>
    /// <seealso cref="RoleEvent"/>
    public class RoleNormalizedNameChanged : RoleEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNormalizedNameChanged"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RoleNormalizedNameChanged()
        {
            NormalizedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNormalizedNameChanged"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="normalizedName">The role new normalized name.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="messageId">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RoleNormalizedNameChanged(string aggregateId, string normalizedName, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
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
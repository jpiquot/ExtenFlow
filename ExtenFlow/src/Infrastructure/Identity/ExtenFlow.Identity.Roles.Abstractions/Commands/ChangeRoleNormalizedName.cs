﻿using System;

namespace ExtenFlow.Identity.Roles
{
    /// <summary>
    /// Change role normalized name
    /// </summary>
    /// <seealso cref="RoleCommand"/>
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
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="normalizedName">The role new normalized name.</param>
        /// <param name="concurrencyStamp">Concurrency stamp used for optimistic concurrency check.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="messageId">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public ChangeRoleNormalizedName(string aggregateId, string normalizedName, string concurrencyStamp, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, concurrencyStamp, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
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
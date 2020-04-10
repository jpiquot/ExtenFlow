using System;

namespace ExtenFlow.Identity.Actors.Role.Command
{
    /// <summary>
    /// Role renamed
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Actors.RoleEvent"/>
    public class RoleRenamed : RoleEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRenamed"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RoleRenamed()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRenamed"/> class.
        /// </summary>
        /// <param name="aggregateId">The role id</param>
        /// <param name="name">The role name.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="dateTime">The date and time.</param>
        public RoleRenamed(string aggregateId, string name, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the new role name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }
    }
}
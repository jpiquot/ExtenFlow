using System;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// Rename role command
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Actors.RoleCommand"/>
    public class RenameRole : RoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameRole"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RenameRole()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenameRole"/> class.
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="name">The name.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="dateTime">The date time.</param>
        public RenameRole(string aggregateId, string name, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(string.Empty, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
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
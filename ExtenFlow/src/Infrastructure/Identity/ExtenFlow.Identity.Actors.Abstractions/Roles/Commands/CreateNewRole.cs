using System;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// Create new role command
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Actors.RoleCommand"/>
    public class CreateNewRole : RoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNewRole"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public CreateNewRole()
        {
            Name = string.Empty;
            NormalizedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNewRole"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="normalizedName">Name of the normalized.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="dateTime">The date time.</param>
        public CreateNewRole(string name, string normalizedName, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(string.Empty, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            Name = name;
            NormalizedName = normalizedName;
        }

        /// <summary>
        /// Gets the new role name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the new role normalized name.
        /// </summary>
        /// <value>The name of the normalized.</value>
        public string NormalizedName { get; }
    }
}
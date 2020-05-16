using System;

namespace ExtenFlow.Identity.Users.Events
{
    /// <summary>
    /// User renamed
    /// </summary>
    /// <seealso cref="UserEvent"/>
    public class UserRenamed : UserEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRenamed"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public UserRenamed()
        {
            Name = string.Empty;
            NormalizedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRenamed"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="name">The user new name.</param>
        /// <param name="normalizedName"></param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public UserRenamed(string aggregateId, string name, string normalizedName, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            Name = name;
            NormalizedName = normalizedName;
        }

        /// <summary>
        /// Gets the new user name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the new normalized user name.
        /// </summary>
        /// <value>The name.</value>
        public string NormalizedName { get; }
    }
}
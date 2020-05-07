using System;

namespace ExtenFlow.Identity.Users.Events
{
    /// <summary>
    /// New user created
    /// </summary>
    /// <seealso cref="UserEvent"/>
    public class NewUserRegistred : UserEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewUserRegistred"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public NewUserRegistred()
        {
            Name = string.Empty;
            NormalizedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewUserRegistred"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="name">The user new name.</param>
        /// <param name="normalizedName">The user new normalized name.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public NewUserRegistred(string aggregateId, string name, string normalizedName, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
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
        /// Gets the new user normalized name.
        /// </summary>
        /// <value>The name of the normalized.</value>
        public string NormalizedName { get; }
    }
}
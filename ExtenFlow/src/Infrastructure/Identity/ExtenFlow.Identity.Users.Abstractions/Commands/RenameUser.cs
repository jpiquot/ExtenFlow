using System;

namespace ExtenFlow.Identity.Users.Commands
{
    /// <summary>
    /// Rename user command
    /// </summary>
    /// <seealso cref="UserCommand"/>
    public class RenameUser : UserCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameUser"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RenameUser()
        {
            Name = string.Empty;
            NormalizedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenameUser"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="name">The user new name.</param>
        /// <param name="normalizedUserName"></param>
        /// <param name="concurrencyStamp">Concurrency stamp used for optimistic concurrency check.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="messageId">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RenameUser(string aggregateId, string name, string normalizedUserName, string concurrencyStamp, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, concurrencyStamp, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            Name = name;
            NormalizedName = normalizedUserName;
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
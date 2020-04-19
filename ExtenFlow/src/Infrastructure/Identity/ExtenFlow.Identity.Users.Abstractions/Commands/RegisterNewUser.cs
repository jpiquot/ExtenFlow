using System;

using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.Users.Commands
{
    /// <summary>
    /// Create new user command
    /// </summary>
    /// <seealso cref="UserCommand"/>
    public class RegisterNewUser : UserCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterNewUser"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RegisterNewUser()
        {
            UserName = string.Empty;
            NormalizedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterNewUser"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="name">The user new name.</param>
        /// <param name="normalizedUserName">The user new normalized name.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="messageId">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RegisterNewUser(string aggregateId, string name, string normalizedUserName, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, null, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
            UserName = name;
            NormalizedName = normalizedUserName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterNewUser"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userId">The user identifier.</param>
        public RegisterNewUser(User user, string userId)
            : this((user == null) ? throw new ArgumentNullException(nameof(user)) : user.Id.ToString(), user.UserName, user.NormalizedUserName, userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Gets the new user normalized name.
        /// </summary>
        /// <value>The name of the normalized.</value>
        public string NormalizedName { get; }

        /// <summary>
        /// Gets the new user name.
        /// </summary>
        /// <value>The name.</value>
        public string UserName { get; }
    }
}
using System;

using ExtenFlow.Domain;

namespace ExtenFlow.Identity.Users.Commands
{
    /// <summary>
    /// Class RegisterNormalizedUserName. Implements the <see
    /// cref="ExtenFlow.Identity.Users.Commands.UserCommand"/>. This command is used to register a
    /// normalized user name.
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Users.Commands.UserCommand"/>
    public class RegisterNormalizedUserName : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterNormalizedUserName"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RegisterNormalizedUserName()
        {
            UserToRegisterId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterNormalizedUserName"/> class.
        /// </summary>
        /// <param name="userToRegisterName">The normalized name of the user to register.</param>
        /// <param name="userToRegisterId">the identifier of the user to be registered</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RegisterNormalizedUserName(string userToRegisterName, string userToRegisterId, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base("NormalizedUserName", userToRegisterName, null, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            UserToRegisterId = userToRegisterId;
        }

        /// <summary>
        /// Gets the identifier of the user to be registered.
        /// </summary>
        /// <value>The name.</value>
        public string UserToRegisterId { get; }
    }
}
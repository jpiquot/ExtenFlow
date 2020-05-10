using System;

using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Users.Commands
{
    /// <summary>
    /// Class DeregisterNormalizedUserName. Implements the <see
    /// cref="ExtenFlow.Identity.Users.Commands.UserCommand"/>. This command is used to deregister a
    /// normalized user name.
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Users.Commands.UserCommand"/>
    public class DeregisterNormalizedUserName : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeregisterNormalizedUserName"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public DeregisterNormalizedUserName()
        {
            UserToDeregisterId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeregisterNormalizedUserName"/> class.
        /// </summary>
        /// <param name="userToDeregisterName">The normalized name of the user to register.</param>
        /// <param name="userToDeregisterId">the identifier of the user to be registered</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public DeregisterNormalizedUserName(string userToDeregisterName, string userToDeregisterId, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base("NormalizedUserName", userToDeregisterName, null, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            UserToDeregisterId = userToDeregisterId;
        }

        /// <summary>
        /// Gets the identifier of the user to be registered.
        /// </summary>
        /// <value>The name.</value>
        public string UserToDeregisterId { get; }
    }
}
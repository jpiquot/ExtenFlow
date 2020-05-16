using System;
using ExtenFlow.Domain;

namespace ExtenFlow.Identity.Users.Events
{
    /// <summary>
    /// Class NormalizedUserNameUnregistred. Implements the <see cref="ExtenFlow.Identity.Users.Events.UserEvent"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Users.Events.UserEvent"/>
    public class NormalizedUserNameDeregistred : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedUserNameDeregistred"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public NormalizedUserNameDeregistred()
        {
            UserNameId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedUserNameDeregistred"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="userNameId">The identifier of the user that holds the name.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public NormalizedUserNameDeregistred(string aggregateId, string userNameId, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base("NormalizedUserName", aggregateId, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            UserNameId = userNameId;
        }

        /// <summary>
        /// Gets the normalized user name.
        /// </summary>
        /// <value>The name.</value>
        public string UserNameId { get; }
    }
}
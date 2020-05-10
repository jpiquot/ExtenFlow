using System;

using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Users.Queries
{
    /// <summary>
    /// Class IsUserNameRegistered./&gt;
    /// </summary>
    public class IsUserNameRegistered : Query<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsUserNameRegistered"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public IsUserNameRegistered()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsUserNameRegistered"/> class.
        /// </summary>
        /// <param name="normalizedName">The normalized role name.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public IsUserNameRegistered(string normalizedName, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base("NormalizedUserName", normalizedName, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
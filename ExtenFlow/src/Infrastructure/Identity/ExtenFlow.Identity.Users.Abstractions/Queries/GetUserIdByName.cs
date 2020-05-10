using System;

namespace ExtenFlow.Identity.Users.Queries
{
    /// <summary>
    /// Class GetUserIdByName.
    /// </summary>
    public class GetUserIdByName : UserQuery<string?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserIdByName"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public GetUserIdByName()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserIdByName"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public GetUserIdByName(string aggregateId, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
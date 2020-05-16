using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ExtenFlow.Identity.Users.Queries
{
    /// <summary>
    /// Class GetUserClaims. Implements the <see cref="UserQuery{UserDetailsViewModel}"/>
    /// </summary>
    /// <seealso cref="UserQuery{UserDetailsViewModel}"/>
    public class GetUserClaims : UserQuery<IList<Claim>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserClaims"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public GetUserClaims()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserClaims"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public GetUserClaims(string aggregateId, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtenFlow.Identity.Users.Commands
{
    /// <summary>
    /// Delete user command
    /// </summary>
    /// <seealso cref="UserCommand"/>
    public class RemoveUserClaims : UserCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserClaims"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RemoveUserClaims()
        {
            Claims = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserClaims"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="claims"></param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="messageId">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RemoveUserClaims(string aggregateId, Dictionary<string, string> claims, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, null, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            Claims = claims ?? throw new ArgumentNullException(nameof(claims));
            if (claims.Count < 1)
            {
                throw new ArgumentException(Properties.Resources.UserClaimListIsEmpty);
            }
            if (claims.Any(p => string.IsNullOrWhiteSpace(p.Key)))
            {
                throw new ArgumentException(Properties.Resources.UserClaimTypeNotDefined, nameof(claims));
            }
            Claims = claims;
        }

        /// <summary>
        /// Gets the claims.
        /// </summary>
        /// <value>The claims.</value>
        public Dictionary<string, string> Claims { get; }
    }
}
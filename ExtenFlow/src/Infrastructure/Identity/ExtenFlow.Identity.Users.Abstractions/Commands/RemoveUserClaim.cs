using System;

namespace ExtenFlow.Identity.Users.Commands
{
    /// <summary>
    /// Delete user command
    /// </summary>
    /// <seealso cref="UserCommand"/>
    public class RemoveUserClaim : UserCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserClaim"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RemoveUserClaim()
        {
            ClaimType = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveUserClaim"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="messageId">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RemoveUserClaim(string aggregateId, string claimType, string? claimValue, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, null, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            if (string.IsNullOrWhiteSpace(claimType))
            {
                throw new ArgumentException(Properties.Resources.UserClaimTypeNotDefined, nameof(claimType));
            }
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        /// <summary>
        /// Gets the type of the user claim.
        /// </summary>
        /// <value>The type of the claim.</value>
        public string ClaimType { get; }

        /// <summary>
        /// Gets the user claim value.
        /// </summary>
        /// <value>The claim value.</value>
        public string? ClaimValue { get; }
    }
}
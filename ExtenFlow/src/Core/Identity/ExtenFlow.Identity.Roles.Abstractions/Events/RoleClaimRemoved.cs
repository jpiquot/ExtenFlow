using System;

namespace ExtenFlow.Identity.Roles.Events
{
    /// <summary>
    /// Role claim removed event
    /// </summary>
    /// <seealso cref="RoleEvent"/>
    public class RoleClaimRemoved : RoleEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimRemoved"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RoleClaimRemoved()
        {
            ClaimType = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimRemoved"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RoleClaimRemoved(string aggregateId, string claimType, string claimValue, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId, id, dateTime ?? DateTimeOffset.Now)
        {
            if (string.IsNullOrWhiteSpace(claimType))
            {
                throw new ArgumentException(Properties.Resources.RoleClaimTypeNotDefined, nameof(claimType));
            }
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        /// <summary>
        /// Gets the type of the role claim.
        /// </summary>
        /// <value>The type of the claim.</value>
        public string ClaimType { get; }

        /// <summary>
        /// Gets the role claim value.
        /// </summary>
        /// <value>The claim value.</value>
        public string? ClaimValue { get; }
    }
}
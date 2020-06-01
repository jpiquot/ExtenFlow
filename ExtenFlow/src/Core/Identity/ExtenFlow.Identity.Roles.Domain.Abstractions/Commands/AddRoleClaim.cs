using System;

namespace ExtenFlow.Identity.Roles.Domain.Commands
{
    /// <summary>
    /// Delete role command
    /// </summary>
    /// <seealso cref="RoleCommand"/>
    public class AddRoleClaim : RoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddRoleClaim"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public AddRoleClaim()
        {
            ClaimType = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddRoleClaim"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="claimType"></param>
        /// <param name="claimValue"></param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="concurrencyCheckStamp">
        /// Concurrency stamp used for optimistic concurrency check.
        /// </param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public AddRoleClaim(string aggregateId, string claimType, string? claimValue, string userId, string concurrencyCheckStamp, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, concurrencyCheckStamp, correlationId, id, dateTime)
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
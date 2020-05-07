using System;

namespace ExtenFlow.Identity.Roles.Commands
{
    /// <summary>
    /// Rename role command
    /// </summary>
    /// <seealso cref="RoleCommand"/>
    public class RenameRole : RoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameRole"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RenameRole()
        {
            Name = string.Empty;
            NormalizedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenameRole"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="name">The role new name.</param>
        /// <param name="normalizedName"></param>
        /// <param name="concurrencyStamp">Concurrency stamp used for optimistic concurrency check.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RenameRole(string aggregateId, string name, string normalizedName, string concurrencyStamp, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, concurrencyStamp, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            Name = name;
            NormalizedName = normalizedName;
        }

        /// <summary>
        /// Gets the new role name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the new normalized role name.
        /// </summary>
        /// <value>The name.</value>
        public string NormalizedName { get; }
    }
}
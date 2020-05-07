using System;

using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.Roles.Commands
{
    /// <summary>
    /// Create new role command
    /// </summary>
    /// <seealso cref="RoleCommand"/>
    public class AddNewRole : RoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewRole"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public AddNewRole()
        {
            Name = string.Empty;
            NormalizedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewRole"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="name">The role new name.</param>
        /// <param name="normalizedName">The role new normalized name.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public AddNewRole(string aggregateId, string name, string normalizedName, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, null, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
            Name = name;
            NormalizedName = normalizedName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewRole"/> class.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="userId">The user identifier.</param>
        public AddNewRole(Role role, string userId)
            : this((role == null) ? throw new ArgumentNullException(nameof(role)) : role.Id.ToString(), role.Name, role.NormalizedName, userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Gets the new role name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the new role normalized name.
        /// </summary>
        /// <value>The name of the normalized.</value>
        public string NormalizedName { get; }
    }
}
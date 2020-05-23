using ExtenFlow.Domain;
using ExtenFlow.Identity.Roles.Events;
using ExtenFlow.Identity.Roles.ValueObjects;
using ExtenFlow.Infrastructure.ValueObjects;

namespace ExtenFlow.Identity.Roles.Domain
{
    /// <summary>
    /// Class RoleEntity. This class cannot be inherited. Implements the <see cref="ExtenFlow.Domain.IEntity"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.IEntity"/>
    internal sealed class RoleEntity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleEntity"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="normalizedName">Name of the normalized.</param>
        /// <param name="concurrencyStamp"></param>
        public RoleEntity(string name, string normalizedName, string? concurrencyStamp = null)
        {
            ConcurrencyStamp = string.IsNullOrWhiteSpace(concurrencyStamp) ?
                new ConcurrencyStamp() :
                new ConcurrencyStamp(concurrencyStamp);
            Name = new Name(name);
            NormalizedName = new NormalizedName(normalizedName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleEntity"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public RoleEntity(RoleState state) : this(state.Name, state.NormalizedName, state.ConcurrencyStamp)
        {
        }

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <value>The name of the entity.</value>
        public static string EntityName => "Role";

        /// <summary>
        /// Gets the concurrency stamp. A random value that should change whenever a role is
        /// persisted to the store.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        public ConcurrencyStamp ConcurrencyStamp { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public Name Name { get; internal set; }

        /// <summary>
        /// Gets or sets the normalized name of the role.
        /// </summary>
        /// <value>The normalized name.</value>
        public NormalizedName NormalizedName { get; internal set; }

        string IEntity.EntityTypeName => EntityName;

        public void Apply(RoleRenamed roleRenamed)
        {
            Name = new Name(roleRenamed.Name);
            NormalizedName = new NormalizedName(roleRenamed.NormalizedName);
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <returns>RoleState.</returns>
        public RoleState GetState()
            => new RoleState(Name.Value, NormalizedName.Value, ConcurrencyStamp.Value);
    }
}
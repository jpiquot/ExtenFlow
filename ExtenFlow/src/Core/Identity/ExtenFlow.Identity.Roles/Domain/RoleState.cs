namespace ExtenFlow.Identity.Roles.Domain
{
    /// <summary>
    /// Class RoleActorState.
    /// </summary>
    public class RoleState
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="normalizedName"></param>
        /// <param name="concurrencyStamp"></param>
        public RoleState(string name, string normalizedName, string concurrencyStamp)
        {
            Name = name;
            NormalizedName = normalizedName;
            ConcurrencyStamp = concurrencyStamp;
        }

        /// <summary>
        /// Gets the concurrency stamp. A random value that should change whenever a role is
        /// persisted to the store.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        public string ConcurrencyStamp { get; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the normalized name of the role.
        /// </summary>
        /// <value>The normalized name.</value>
        public string NormalizedName { get; }
    }
}
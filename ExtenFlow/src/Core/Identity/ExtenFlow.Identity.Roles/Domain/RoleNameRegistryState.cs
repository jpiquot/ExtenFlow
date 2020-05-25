namespace ExtenFlow.Identity.Roles.Domain
{
    /// <summary>
    /// Class RoleNameRegistryState.
    /// </summary>
    public class RoleNameRegistryState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNameRegistryState"/> class.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="concurrencyCheckStamp">The concurrency check stamp.</param>
        public RoleNameRegistryState(string roleId, string concurrencyCheckStamp)
        {
            ConcurrencyCheckStamp = concurrencyCheckStamp;
            RoleId = roleId;
        }

        /// <summary>
        /// Gets the concurrency stamp. A random value that should change whenever a role is
        /// persisted to the store.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        public string ConcurrencyCheckStamp { get; }

        /// <summary>
        /// Gets the role identifier.
        /// </summary>
        /// <value>The role identifier.</value>
        public string RoleId { get; }
    }
}
namespace ExtenFlow.Identity.Roles
{
    /// <summary>
    /// Struct AggregateName
    /// </summary>
    public static class AggregateName
    {
        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>The role.</value>
        public static string Role => nameof(Role);

        /// <summary>
        /// Gets the role collection.
        /// </summary>
        /// <value>The role collection.</value>
        public static string RoleCollection => nameof(Role) + "Collection";

        /// <summary>
        /// Gets the role name registry.
        /// </summary>
        /// <value>The role name registry.</value>
        public static string RoleNameRegistry => nameof(RoleNameRegistry);
    }
}
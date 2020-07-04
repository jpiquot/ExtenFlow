namespace ExtenFlow.IdentityServer.Domain
{
    /// <summary>
    /// Aggregate names
    /// </summary>
    public static class AggregateName
    {
        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <value>The client.</value>
        public static string Client => nameof(Client);

        /// <summary>
        /// Gets the device flow.
        /// </summary>
        /// <value>The device flow.</value>
        public static string DeviceFlow => nameof(DeviceFlow);

        /// <summary>
        /// Gets the persisted grant.
        /// </summary>
        /// <value>The persisted grant.</value>
        public static string PersistedGrant => nameof(PersistedGrant);

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <value>The resource.</value>
        public static string Resource => nameof(Resource);
    }
}
﻿namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class ExternalProvider.
    /// </summary>
    public class ExternalProvider
    {
        /// <summary>
        /// Gets or sets the authentication scheme.
        /// </summary>
        /// <value>The authentication scheme.</value>
        public string? AuthenticationScheme { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string? DisplayName { get; set; }
    }
}
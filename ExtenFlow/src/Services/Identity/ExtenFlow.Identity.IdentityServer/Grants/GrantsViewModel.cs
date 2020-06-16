using System;
using System.Collections.Generic;

namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class GrantsViewModel.
    /// </summary>
    public class GrantsViewModel
    {
        /// <summary>
        /// Gets or sets the grants.
        /// </summary>
        /// <value>The grants.</value>
        public IEnumerable<GrantViewModel>? Grants { get; set; }
    }

    /// <summary>
    /// Class GrantViewModel.
    /// </summary>
    public class GrantViewModel
    {
        /// <summary>
        /// Gets or sets the API grant names.
        /// </summary>
        /// <value>The API grant names.</value>
        public IEnumerable<string>? ApiGrantNames { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public string? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client logo URL.
        /// </summary>
        /// <value>The client logo URL.</value>
        public Uri? ClientLogoUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the client.
        /// </summary>
        /// <value>The name of the client.</value>
        public string? ClientName { get; set; }

        /// <summary>
        /// Gets or sets the client URL.
        /// </summary>
        /// <value>The client URL.</value>
        public Uri? ClientUrl { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>The created.</value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Gets or sets the identity grant names.
        /// </summary>
        /// <value>The identity grant names.</value>
        public IEnumerable<string>? IdentityGrantNames { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class ConsentViewModel. Implements the <see cref="ExtenFlow.Identity.IdentityServer.ConsentInputModel"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.IdentityServer.ConsentInputModel"/>
    public class ConsentViewModel : ConsentInputModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether [allow remember consent].
        /// </summary>
        /// <value><c>true</c> if [allow remember consent]; otherwise, <c>false</c>.</value>
        public bool AllowRememberConsent { get; set; }

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
        /// Gets or sets the identity scopes.
        /// </summary>
        /// <value>The identity scopes.</value>
        public IEnumerable<ScopeViewModel>? IdentityScopes { get; set; }

        /// <summary>
        /// Gets or sets the resource scopes.
        /// </summary>
        /// <value>The resource scopes.</value>
        public IEnumerable<ScopeViewModel>? ResourceScopes { get; set; }
    }
}
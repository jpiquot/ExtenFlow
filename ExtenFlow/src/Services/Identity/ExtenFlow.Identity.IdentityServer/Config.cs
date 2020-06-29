using System;
using System.Collections.Generic;

using IdentityServer4.Models;

namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class Config.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Gets the API resources.
        /// </summary>
        /// <value>The API resources.</value>
        public static IEnumerable<ApiResource> ApiResources
            => Array.Empty<ApiResource>();

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <value>The clients.</value>
        public static IEnumerable<Client> Clients
            => new Client[]
            {
                new Client{
                    ClientId = "identity-web",
                    ClientName="Identity Web Server",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = {"http://localhost:25658/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:25658/signout-callback-oidc"},
                    BackChannelLogoutUri = "http://localhost:25658/signout-oidc",
                    FrontChannelLogoutUri = "http://localhost:25658/signout-oidc",
                    AllowedScopes = { "openid", "email", "profile"}
                },
                new Client{
                    ClientId = "identity-actors",
                    ClientName="Identity Actors",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { "http://localhost:25657/signin-oidc" },
                    AllowedScopes = { "openid"}
                },
                new Client{
                    ClientId = "identity-services",
                    ClientName="Identity Services",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { "http://localhost:25656/signin-oidc" },
                    AllowedScopes = { "openid"}
                }
            };

        /// <summary>
        /// Gets the identity resources.
        /// </summary>
        /// <value>The identity resources.</value>
        public static IEnumerable<IdentityResource> IdentityResources
            => new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Address()
            };
    }
}
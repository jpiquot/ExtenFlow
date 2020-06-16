using System;
using System.Threading.Tasks;

using IdentityServer4.Stores;

using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1054 // Uri parameters should not be strings

namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class Extensions.
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Determines whether the client is configured to use PKCE.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public static async Task<bool> IsPkceClientAsync(this IClientStore store, string clientId)
        {
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                IdentityServer4.Models.Client? client = await store.FindEnabledClientByIdAsync(clientId);
                return client?.RequirePkce == true;
            }

            return false;
        }

        /// <summary>
        /// Loadings the page.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="redirectTo">The redirect URI.</param>
        /// <returns>IActionResult.</returns>
        public static IActionResult LoadingPage(this Controller controller, string viewName, string? redirectTo)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            controller.HttpContext.Response.StatusCode = 200;
            controller.HttpContext.Response.Headers["Location"] = "";

            return controller.View(viewName, new RedirectViewModel { RedirectUrl = redirectTo.ToUri() });
        }

        /// <summary>
        /// Converts a string to an Uri object.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The Uri instance or null if the string is null or empty.</returns>
        public static Uri? ToUri(this string? url)
            => string.IsNullOrWhiteSpace(url) ? null : new Uri(url);
    }
}
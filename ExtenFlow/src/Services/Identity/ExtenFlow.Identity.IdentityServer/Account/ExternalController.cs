using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using IdentityModel;

using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class ExternalController. Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller"/>
    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<ExternalController> _logger;
        private readonly TestUserStore _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalController"/> class.
        /// </summary>
        /// <param name="interaction">The interaction.</param>
        /// <param name="clientStore">The client store.</param>
        /// <param name="events">The events.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="users">The users.</param>
        public ExternalController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            ILogger<ExternalController> logger,
            TestUserStore? users = null)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g.
            // ASP.NET Identity)
            _users = users ?? new TestUserStore(TestUsers.Users);

            _interaction = interaction;
            _clientStore = clientStore;
            _logger = logger;
            _events = events;
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            AuthenticateResult? result = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception(Properties.Resources.ExternalAuthenticationError);
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                IEnumerable<string>? externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
                // External claims: {%1}.
                _logger.LogDebug(string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.ExternalClaimsLog,
                    string.Join(", ", externalClaims)));
            }

            // lookup our user and external provider info
            (TestUser user, string provider, string providerUserId, IEnumerable<Claim> claims) = FindUserFromExternalProvider(result);
            if (user == null)
            {
                // this might be where you might initiate a custom workflow for user registration in
                // this sample we don't show how that would be done, as our sample implementation
                // simply auto-provisions new external user
                user = AutoProvisionUser(provider, providerUserId, claims);
            }

            // this allows us to collect any additional claims or properties for the specific
            // protocols used and store them in the local auth cookie. this is typically used to
            // store data needed for signout from those protocols.
            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallbackForOidc(result, additionalLocalClaims, localSignInProps);
            //ProcessLoginCallbackForWsFed(result, additionalLocalClaims, localSignInProps);
            //ProcessLoginCallbackForSaml2p(result, additionalLocalClaims, localSignInProps);

            // issue authentication cookie for user
            var isuser = new IdentityServerUser(user.SubjectId)
            {
                DisplayName = user.Username,
                IdentityProvider = provider,
                AdditionalClaims = additionalLocalClaims
            };

            await HttpContext.SignInAsync(isuser, localSignInProps);

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // retrieve return URL
            string? returnTo = result.Properties.Items["returnUrl"] ?? "~/";

            // check if external login is in the context of an OIDC request
            AuthorizationRequest? context = await _interaction.GetAuthorizationContextAsync(returnTo);
            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.SubjectId, user.Username, true, context?.Client?.ClientId));

            if (context?.Client != null)
            {
                if (await _clientStore.IsPkceClientAsync(context.Client.ClientId))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage("Redirect", returnTo);
                }
            }

            return Redirect(returnTo);
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Challenge(string provider, string returnTo)
        {
            if (string.IsNullOrEmpty(returnTo))
            {
                returnTo = "~/";
            }

            // validate returnTo - either it is a valid OIDC URL or back to a local page
            if (Url.IsLocalUrl(returnTo) == false && _interaction.IsValidReturnUrl(returnTo) == false)
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception(Properties.Resources.InvalidReturnUrl);
            }

            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                // windows authentication needs special handling
                return await ProcessWindowsLoginAsync(returnTo);
            }
            else
            {
                // start challenge and roundtrip the return URL and scheme
                var props = new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(Callback)),
                    Items =
                    {
                        { "returnUrl", returnTo },
                        { "scheme", provider },
                    }
                };

                return Challenge(props, provider);
            }
        }

        private static void ProcessLoginCallbackForOidc(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over so we can use it for
            // single sign-out
            Claim? sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            string? id_token = externalResult.Properties.GetTokenValue("id_token");
            if (id_token != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = id_token } });
            }
        }

        private TestUser AutoProvisionUser(string provider, string providerUserId, IEnumerable<Claim> claims)
        {
            TestUser? user = _users.AutoProvisionUser(provider, providerUserId, claims.ToList());
            return user;
        }

        private (TestUser user, string provider, string providerUserId, IEnumerable<Claim> claims) FindUserFromExternalProvider(AuthenticateResult result)
        {
            ClaimsPrincipal? externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider) the most
            // common claim type for that are the sub claim and the NameIdentifier depending on the
            // external provider, some other claim type might be used
            Claim? userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception(Properties.Resources.UnknownUserid);

            // remove the user id claim so we don't include it as an extra claim if/when we
            // provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            string? provider = result.Properties.Items["scheme"];
            string? providerUserId = userIdClaim.Value;

            // find external user
            TestUser? user = _users.FindByExternalProvider(provider, providerUserId);

            return (user, provider, providerUserId, claims);
        }

        private async Task<IActionResult> ProcessWindowsLoginAsync(string returnTo)
        {
            // see if windows auth has already been requested and succeeded
            AuthenticateResult? result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if (result?.Principal is WindowsPrincipal wp)
            {
                // we will issue the external cookie and then redirect the user back to the external
                // callback, in essence, treating windows auth the same as any other external
                // authentication mechanism
                var props = new AuthenticationProperties()
                {
                    RedirectUri = Url.Action("Callback"),
                    Items =
                    {
                        { "returnUrl", returnTo },
                        { "scheme", AccountOptions.WindowsAuthenticationSchemeName },
                    }
                };

                var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
                id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.FindFirst(ClaimTypes.PrimarySid).Value));
                id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                // add the groups as claims -- be careful if the number of groups is too large
                if (AccountOptions.IncludeWindowsGroups && wp.Identity is WindowsIdentity wi)
                {
                    IdentityReferenceCollection? groups = wi.Groups.Translate(typeof(NTAccount));
                    IEnumerable<Claim>? roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
                    id.AddClaims(roles);
                }

                await HttpContext.SignInAsync(
                    IdentityServerConstants.ExternalCookieAuthenticationScheme,
                    new ClaimsPrincipal(id),
                    props);
                return Redirect(props.RedirectUri);
            }
            else
            {
                // trigger windows auth since windows auth don't support the redirect uri, this URL
                // is re-triggered when we call challenge
                return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
            }
        }
    }
}
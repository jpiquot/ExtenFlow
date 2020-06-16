using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// This controller processes the consent UI
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    public class ConsentController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<ConsentController> _logger;
        private readonly IResourceStore _resourceStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsentController"/> class.
        /// </summary>
        /// <param name="interaction">The interaction.</param>
        /// <param name="clientStore">The client store.</param>
        /// <param name="resourceStore">The resource store.</param>
        /// <param name="events">The events.</param>
        /// <param name="logger">The logger.</param>
        public ConsentController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            IEventService events,
            ILogger<ConsentController> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _events = events;
            _logger = logger;
        }

        /// <summary>
        /// Shows the consent screen
        /// </summary>
        /// <param name="returnTo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string returnTo)
        {
            ConsentViewModel? vm = await BuildViewModelAsync(returnTo);
            if (vm != null)
            {
                return View("Index", vm);
            }

            return View("Error");
        }

        /// <summary>
        /// Handles the consent screen postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            ProcessConsentResult result = await ProcessConsent(model ?? throw new ArgumentNullException(nameof(model)));

            if (result.IsRedirect)
            {
                string? redirect = result.RedirectUri?.ToString() ?? throw new NullReferenceException(Properties.Resources.RedirectUrlNotDefined);
                if (await _clientStore.IsPkceClientAsync(result.ClientId ?? string.Empty))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage("Redirect", redirect);
                }

                return Redirect(redirect);
            }

            if (result.HasValidationError == true)
            {
                ModelState.AddModelError(string.Empty, result.ValidationError);
            }

            if (result.ShowView)
            {
                return View("Index", result.ViewModel);
            }

            return View("Error");
        }

        /*****************************************/
        /* helper APIs for the ConsentController */
        /*****************************************/

        private static ConsentViewModel CreateConsentViewModel(
            ConsentInputModel? model, string returnTo,
            Client client, Resources resources)
        {
            var vm = new ConsentViewModel
            {
                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),

                ReturnUrl = new Uri(returnTo),

                ClientName = client.ClientName ?? client.ClientId,
                ClientUrl = client.ClientUri?.ToUri(),
                ClientLogoUrl = client.LogoUri?.ToUri(),
                AllowRememberConsent = client.AllowRememberConsent
            };

            vm.IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
            vm.ResourceScopes = resources.ApiScopes.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
            if (ConsentOptions.EnableOfflineAccess && resources.OfflineAccess)
            {
                vm.ResourceScopes = vm.ResourceScopes.Union(new ScopeViewModel[] {
                    GetOfflineAccessScope(vm.ScopesConsented.Contains(IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess) || model == null)
                });
            }

            return vm;
        }

        private static ScopeViewModel CreateScopeViewModel(ApiScope identity, bool check)
                    => new ScopeViewModel
                    {
                        Name = identity.Name,
                        DisplayName = identity.DisplayName,
                        Description = identity.Description,
                        Emphasize = identity.Emphasize,
                        Required = identity.Required,
                        Checked = check || identity.Required
                    };

        private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
            => new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };

        private static ScopeViewModel GetOfflineAccessScope(bool check)
            => new ScopeViewModel
            {
                Name = IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = ConsentOptions.OfflineAccessDisplayName,
                Description = ConsentOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
            };

        private async Task<ConsentViewModel?> BuildViewModelAsync(string? returnTo, ConsentInputModel? model = null)
        {
            AuthorizationRequest? request = await _interaction.GetAuthorizationContextAsync(returnTo);
            if (request != null)
            {
                Client? client = await _clientStore.FindEnabledClientByIdAsync(request.Client.ClientId);
                if (client != null)
                {
                    Resources? resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ValidatedResources.RawScopeValues);
                    if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        return CreateConsentViewModel(model, returnTo ?? string.Empty, client, resources);
                    }
                    else
                    {
                        _logger.LogError(string.Format(CultureInfo.CurrentCulture, Properties.Resources.NoScopeMatching, string.Join(", ", request.ValidatedResources.RawScopeValues)));
                    }
                }
                else
                {
                    _logger.LogError(string.Format(CultureInfo.CurrentCulture, Properties.Resources.InvalidClientId, request.Client.ClientId));
                }
            }
            else
            {
                _logger.LogError(string.Format(CultureInfo.CurrentCulture, Properties.Resources.ConsentRequestMismatch, returnTo));
            }

            return null;
        }

        private async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
        {
            var result = new ProcessConsentResult();

            // validate return url is still valid
            AuthorizationRequest? request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl?.ToString());
            if (request == null)
            {
                return result;
            }

            ConsentResponse? grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (model?.Button == "no")
            {
                grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };

                // emit event
                await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
            }
            // user clicked 'yes' - validate the data
            else if (model?.Button == "yes")
            {
                // if the user consented to some scope, build the response model
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    System.Collections.Generic.IEnumerable<string>? scopes = model.ScopesConsented;
                    if (ConsentOptions.EnableOfflineAccess == false)
                    {
                        scopes = scopes.Where(x => x != IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess);
                    }

                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesValuesConsented = scopes
                    };

                    // emit event
                    await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
                }
                else
                {
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                }
            }
            else
            {
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            }

            if (grantedConsent != null)
            {
                // communicate outcome of consent back to identityserver
                await _interaction.GrantConsentAsync(request, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = model?.ReturnUrl;
                result.ClientId = request.Client.ClientId;
            }
            else
            {
                // we need to redisplay the consent UI
                result.ViewModel = await BuildViewModelAsync(model?.ReturnUrl?.ToString(), model);
            }

            return result;
        }
    }
}
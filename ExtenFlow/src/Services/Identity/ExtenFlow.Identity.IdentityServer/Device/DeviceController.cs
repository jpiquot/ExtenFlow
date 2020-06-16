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

namespace ExtenFlow.Identity.IdentityServer.Device
{
    /// <summary>
    /// Class DeviceController. Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller"/>
    [Authorize]
    [SecurityHeaders]
    public class DeviceController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IDeviceFlowInteractionService _interaction;
        private readonly ILogger<DeviceController> _logger;
        private readonly IResourceStore _resourceStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceController"/> class.
        /// </summary>
        /// <param name="interaction">The interaction.</param>
        /// <param name="clientStore">The client store.</param>
        /// <param name="resourceStore">The resource store.</param>
        /// <param name="eventService">The event service.</param>
        /// <param name="logger">The logger.</param>
        public DeviceController(
            IDeviceFlowInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            IEventService eventService,
            ILogger<DeviceController> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _events = eventService;
            _logger = logger;
        }

        /// <summary>
        /// Creates the scope view model.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="check">if set to <c>true</c> [check].</param>
        /// <returns>ScopeViewModel.</returns>
        public static ScopeViewModel CreateScopeViewModel(ApiScope scope, bool check)
            => new ScopeViewModel
            {
                Name = scope?.Name,
                DisplayName = scope?.DisplayName,
                Description = scope?.Description,
                Emphasize = scope?.Emphasize == true,
                Required = scope?.Required == true,
                Checked = check || scope?.Required == true
            };

        /// <summary>
        /// Callbacks the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>IActionResult.</returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Callback(DeviceAuthorizationInputModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            ProcessConsentResult? result = await ProcessConsent(model);
            return result.HasValidationError ? View("Error") : View("Success");
        }

        /// <summary>
        /// Indexes the specified user code.
        /// </summary>
        /// <param name="userCode">The user code.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "user_code")] string userCode)
        {
            if (string.IsNullOrWhiteSpace(userCode))
            {
                return View("UserCodeCapture");
            }

            DeviceAuthorizationViewModel? vm = await BuildViewModelAsync(userCode);
            if (vm == null)
            {
                return View("Error");
            }

            vm.ConfirmUserCode = true;
            return View("UserCodeConfirmation", vm);
        }

        /// <summary>
        /// Users the code capture.
        /// </summary>
        /// <param name="userCode">The user code.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserCodeCapture(string userCode)
        {
            DeviceAuthorizationViewModel? vm = await BuildViewModelAsync(userCode);
            return vm == null ? View("Error") : View("UserCodeConfirmation", vm);
        }

        private static DeviceAuthorizationViewModel CreateConsentViewModel(string? userCode, DeviceAuthorizationInputModel? model, Client client, Resources resources)
        {
            var vm = new DeviceAuthorizationViewModel
            {
                UserCode = userCode,

                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),

                ClientName = client.ClientName ?? client.ClientId,
                ClientUrl = client.ClientUri.ToUri(),
                ClientLogoUrl = client.LogoUri.ToUri(),
                AllowRememberConsent = client.AllowRememberConsent
            };

            vm.IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
            vm.ResourceScopes = resources.ApiScopes.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
            if (ConsentOptions.EnableOfflineAccess && resources.OfflineAccess)
            {
                vm.ResourceScopes = vm.ResourceScopes.Union(new[]
                {
                    GetOfflineAccessScope(vm.ScopesConsented.Contains(IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess) || model == null)
                });
            }

            return vm;
        }

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

        /// <summary>
        /// build view model as an asynchronous operation.
        /// </summary>
        /// <param name="userCode">The user code.</param>
        /// <param name="model">The model.</param>
        /// <returns>DeviceAuthorizationViewModel.</returns>
        private async Task<DeviceAuthorizationViewModel?> BuildViewModelAsync(string? userCode, DeviceAuthorizationInputModel? model = null)
        {
            DeviceFlowAuthorizationRequest? request = await _interaction.GetAuthorizationContextAsync(userCode);
            if (request != null)
            {
                Client? client = await _clientStore.FindEnabledClientByIdAsync(request.Client.ClientId);
                if (client != null)
                {
                    Resources? resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ValidatedResources.RawScopeValues);
                    if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        return CreateConsentViewModel(userCode, model, client, resources);
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

            return null;
        }

        private async Task<ProcessConsentResult> ProcessConsent(DeviceAuthorizationInputModel model)
        {
            var result = new ProcessConsentResult();

            DeviceFlowAuthorizationRequest? request = await _interaction.GetAuthorizationContextAsync(model.UserCode);
            if (request == null)
            {
                return result;
            }

            ConsentResponse? grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (model.Button == "no")
            {
                grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };

                // emit event
                await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
            }
            // user clicked 'yes' - validate the data
            else if (model.Button == "yes")
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
                await _interaction.HandleRequestAsync(model.UserCode, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = model.ReturnUrl;
                result.ClientId = request.Client.ClientId;
            }
            else
            {
                // we need to redisplay the consent UI
                result.ViewModel = await BuildViewModelAsync(model.UserCode, model);
            }

            return result;
        }
    }
}
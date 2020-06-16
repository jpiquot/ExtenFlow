// Copyright (c) Brock Allen & Dominick Baier. All rights reserved. Licensed under the Apache
// License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;

using IdentityServer4.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class HomeController. Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller"/>
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="interaction">The interaction.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="logger">The logger.</param>
        public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger)
        {
            _interaction = interaction;
            _environment = environment;
            _logger = logger;
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            IdentityServer4.Models.ErrorMessage? message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>IActionResult.</returns>
        public IActionResult Index()
        {
            if (_environment.IsDevelopment())
            {
                // only show in development
                return View();
            }

            _logger.LogInformation(Properties.Resources.NoHomePageInProduction);
            return NotFound();
        }
    }
}
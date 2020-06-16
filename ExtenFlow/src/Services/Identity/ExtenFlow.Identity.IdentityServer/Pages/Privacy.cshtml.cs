using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ExtenFlow.Identity.IdentityServer.Pages
{
    /// <summary>
    /// Class PrivacyModel. Implements the <see cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel"/>
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivacyModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }
    }
}
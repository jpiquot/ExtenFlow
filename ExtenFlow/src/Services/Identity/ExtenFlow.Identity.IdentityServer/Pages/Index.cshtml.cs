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
    /// Class IndexModel. Implements the <see cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel"/>
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
    }
}
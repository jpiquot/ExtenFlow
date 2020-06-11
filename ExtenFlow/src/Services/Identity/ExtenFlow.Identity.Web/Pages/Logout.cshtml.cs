using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExtenFlow.Identity.Web.Pages
{
    /// <summary>
    /// Class LogoutModel. Implements the <see cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel"/>
    public class LogoutModel : PageModel
    {
        /// <summary>
        /// Called when [get asynchronous].
        /// </summary>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect(Url.Content("~/"));
        }
    }
}
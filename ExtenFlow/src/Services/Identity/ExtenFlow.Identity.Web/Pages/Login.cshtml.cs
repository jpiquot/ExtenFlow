using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExtenFlow.Identity.Web.Pages
{
    /// <summary>
    /// Class LoginModel. Implements the <see cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel"/>
    public class LoginModel : PageModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [BindProperty]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [BindProperty]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        /// <summary>
        /// Called when [get].
        /// </summary>
#pragma warning disable CA1822 // Mark members as static

        public async Task<IActionResult> OnPostAsync()
#pragma warning restore CA1822 // Mark members as static
        {
            if (!(Email == "login" && Password == "password"))
            {
                return Page();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Jerome"),
                new Claim(ClaimTypes.Email, "jpiquot@fiveforty.fr")
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            return LocalRedirect(Url.Content("~/"));
        }
    }
}
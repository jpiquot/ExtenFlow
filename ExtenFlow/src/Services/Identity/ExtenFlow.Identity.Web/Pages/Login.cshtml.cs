using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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

        public void OnGet()
#pragma warning restore CA1822 // Mark members as static
        {
        }
    }
}
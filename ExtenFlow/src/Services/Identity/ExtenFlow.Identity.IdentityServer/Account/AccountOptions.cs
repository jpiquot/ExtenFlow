using System;

#pragma warning disable CA2211 // Non-constant fields should not be visible

namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class AccountOptions.
    /// </summary>
    public static class AccountOptions
    {
        /// <summary>
        /// The windows authentication scheme name
        /// </summary>
        public static readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;

        /// <summary>
        /// The allow local login/
        /// </summary>
        public static bool AllowLocalLogin = true;

        /// <summary>
        /// The allow remember login
        /// </summary>
        public static bool AllowRememberLogin = true;

        /// <summary>
        /// The automatic redirect after sign out
        /// </summary>
        public static bool AutomaticRedirectAfterSignOut = false;

        /// <summary>
        /// The include windows groups
        /// </summary>
        /// <remarks>if user uses windows auth, should we load the groups from windows.</remarks>
        public static bool IncludeWindowsGroups = false;

        /// <summary>
        /// The invalid credentials error message
        /// </summary>
        public static string InvalidCredentialsErrorMessage = Properties.Resources.InvalidCredentials;

        /// <summary>
        /// The remember me login duration
        /// </summary>
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        /// <summary>
        /// The show logout prompt
        /// </summary>
        public static bool ShowLogoutPrompt = true;
    }
}
#pragma warning disable CA2211 // Non-constant fields should not be visible

namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class ConsentOptions.
    /// </summary>
    public static class ConsentOptions
    {
        /// <summary>
        /// The invalid selection error message
        /// </summary>
        public static readonly string InvalidSelectionErrorMessage = Properties.Resources.InvalidSelection;

        /// <summary>
        /// The must choose one error message
        /// </summary>
        public static readonly string MustChooseOneErrorMessage = Properties.Resources.PickOnePermission;

        /// <summary>
        /// The enable offline access
        /// </summary>
        public static bool EnableOfflineAccess = true;

        /// <summary>
        /// The offline access description
        /// </summary>
        public static string OfflineAccessDescription = Properties.Resources.AccessApplicationOffline;

        /// <summary>
        /// The offline access display name
        /// </summary>
        public static string OfflineAccessDisplayName = Properties.Resources.OfflineAccess;
    }
}
namespace ExtenFlow.Identity.IdentityServer
{
    /// <summary>
    /// Class LogoutViewModel. Implements the <see cref="ExtenFlow.Identity.IdentityServer.LogoutInputModel"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.IdentityServer.LogoutInputModel"/>
    public class LogoutViewModel : LogoutInputModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether [show logout prompt].
        /// </summary>
        /// <value><c>true</c> if [show logout prompt]; otherwise, <c>false</c>.</value>
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
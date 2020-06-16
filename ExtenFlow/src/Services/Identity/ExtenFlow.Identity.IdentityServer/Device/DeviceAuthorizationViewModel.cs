namespace ExtenFlow.Identity.IdentityServer.Device
{
    /// <summary>
    /// Class DeviceAuthorizationViewModel. Implements the <see cref="ExtenFlow.Identity.IdentityServer.ConsentViewModel"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.IdentityServer.ConsentViewModel"/>
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether [confirm user code].
        /// </summary>
        /// <value><c>true</c> if [confirm user code]; otherwise, <c>false</c>.</value>
        public bool ConfirmUserCode { get; set; }

        /// <summary>
        /// Gets or sets the user code.
        /// </summary>
        /// <value>The user code.</value>
        public string? UserCode { get; set; }
    }
}
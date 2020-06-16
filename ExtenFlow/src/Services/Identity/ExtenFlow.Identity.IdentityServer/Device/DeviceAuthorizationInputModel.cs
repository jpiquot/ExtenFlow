namespace ExtenFlow.Identity.IdentityServer.Device
{
    /// <summary>
    /// Class DeviceAuthorizationInputModel. Implements the <see cref="ExtenFlow.Identity.IdentityServer.ConsentInputModel"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.IdentityServer.ConsentInputModel"/>
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        /// <summary>
        /// Gets or sets the user code.
        /// </summary>
        /// <value>The user code.</value>
        public string? UserCode { get; set; }
    }
}
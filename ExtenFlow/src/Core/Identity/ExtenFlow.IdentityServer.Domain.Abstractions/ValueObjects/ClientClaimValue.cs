using ExtenFlow.Infrastructure;

namespace ExtenFlow.IdentityServer.Domain.ValueObjects
{
    /// <summary>
    /// Class ClaimValueValue
    /// </summary>
    public class ClientClaimValue : ValueObject<string?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientClaimValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parentName"></param>
        /// <param name="propertyName"></param>
        public ClientClaimValue(string value, string? parentName = null, string? propertyName = null)
            : base(value, new ClientClaimValueValidator(parentName, propertyName))
        {
        }
    }
}
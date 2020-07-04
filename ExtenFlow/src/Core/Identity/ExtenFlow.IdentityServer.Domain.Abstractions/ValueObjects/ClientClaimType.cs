using ExtenFlow.Infrastructure;

namespace ExtenFlow.IdentityServer.Domain.ValueObjects
{
    /// <summary>
    /// Class ClaimTypeValue
    /// </summary>
    public class ClientClaimType : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientClaimType"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parentName"></param>
        /// <param name="propertyName"></param>
        public ClientClaimType(string value, string? parentName = null, string? propertyName = null)
            : base(value, new ClientClaimTypeValidator(parentName, propertyName))
        {
        }
    }
}
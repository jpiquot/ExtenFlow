using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.IdentityServer.Domain.ValueObjects
{
    /// <summary>
    /// Class ClaimValueValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class ClientClaimValueValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientClaimValueValidator"/> class.
        /// </summary>
        /// <param name="parentName">Name of the parent.</param>
        /// <param name="propertyName">Name of the property.</param>
        public ClientClaimValueValidator(string? parentName = null, string? propertyName = null)
            : base(parentName, propertyName, true, 0, int.MaxValue, true)
        {
        }
    }
}
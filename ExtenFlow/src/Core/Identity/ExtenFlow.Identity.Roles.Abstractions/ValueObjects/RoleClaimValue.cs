using ExtenFlow.Infrastructure;

namespace ExtenFlow.Identity.Roles.ValueObjects
{
    /// <summary>
    /// Class ClaimValueValue
    /// </summary>
    public class RoleClaimValue : ValueObject<string?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parentName"></param>
        /// <param name="propertyName"></param>
        public RoleClaimValue(string value, string? parentName = null, string? propertyName = null)
            : base(value, new RoleClaimValueValidator(parentName, propertyName))
        {
        }
    }
}
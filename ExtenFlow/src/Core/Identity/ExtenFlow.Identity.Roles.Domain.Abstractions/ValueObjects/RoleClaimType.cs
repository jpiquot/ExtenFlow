using ExtenFlow.Infrastructure;

namespace ExtenFlow.Identity.Roles.Domain.ValueObjects
{
    /// <summary>
    /// Class ClaimTypeValue
    /// </summary>
    public class RoleClaimType : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimType"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parentName"></param>
        /// <param name="propertyName"></param>
        public RoleClaimType(string value, string? parentName = null, string? propertyName = null)
            : base(value, new RoleClaimTypeValidator(parentName, propertyName))
        {
        }
    }
}
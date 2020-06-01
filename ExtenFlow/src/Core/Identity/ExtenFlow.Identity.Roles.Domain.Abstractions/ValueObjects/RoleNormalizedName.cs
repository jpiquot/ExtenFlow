using ExtenFlow.Infrastructure;

namespace ExtenFlow.Identity.Roles.Domain.ValueObjects
{
    /// <summary>
    /// Class NormalizedNameValue
    /// </summary>
    public class RoleNormalizedName : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNormalizedName"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parentName"></param>
        /// <param name="propertyName"></param>
        public RoleNormalizedName(string value, string? parentName = null, string? propertyName = null)
            : base(value, new RoleNameValidator(parentName, propertyName))
        {
        }
    }
}
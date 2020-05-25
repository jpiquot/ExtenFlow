using ExtenFlow.Infrastructure;

namespace ExtenFlow.Identity.Roles.ValueObjects
{
    /// <summary>
    /// Class NameValue
    /// </summary>
    public class RoleName : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleName"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parentName"></param>
        /// <param name="propertyName"></param>
        public RoleName(string value, string? parentName = null, string? propertyName = null)
            : base(value, new RoleNameValidator(parentName, propertyName))
        {
        }
    }
}
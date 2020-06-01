using ExtenFlow.Infrastructure;

namespace ExtenFlow.Identity.Roles.Domain.ValueObjects
{
    /// <summary>
    /// Class NameValue
    /// </summary>
    public class RoleId : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleName"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parentName"></param>
        /// <param name="propertyName"></param>
        public RoleId(string value, string? parentName = null, string? propertyName = null)
            : base(value, new RoleIdValidator(parentName, propertyName))
        {
        }
    }
}
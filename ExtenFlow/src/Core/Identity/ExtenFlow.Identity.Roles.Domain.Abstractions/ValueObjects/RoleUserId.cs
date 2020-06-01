using ExtenFlow.Infrastructure;

namespace ExtenFlow.Identity.Roles.Domain.ValueObjects
{
    /// <summary>
    /// Class RoleUserId. Implements the <see cref="ValueObject{T}"/>
    /// </summary>
    /// <seealso cref="ValueObject{T}"/>
    public class RoleUserId : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleUserId"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        public RoleUserId(string value, string? instanceName = null, string? propertyName = null)
            : base(value, new RoleUserIdValidator(instanceName, propertyName))
        {
        }
    }
}
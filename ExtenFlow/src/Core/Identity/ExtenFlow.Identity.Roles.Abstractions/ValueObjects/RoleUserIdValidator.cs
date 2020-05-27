using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.ValueObjects
{
    /// <summary>
    /// Class RoleUserIdValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class RoleUserIdValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleUserIdValidator"/> class.
        /// </summary>
        /// <param name="instanceName">Name of the parent.</param>
        /// <param name="propertyName">Name of the property.</param>
        public RoleUserIdValidator(string? instanceName = null, string? propertyName = null)
            : base(instanceName, propertyName, false, 1, 22, false)
        {
        }
    }
}
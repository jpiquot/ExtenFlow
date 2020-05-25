using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.ValueObjects
{
    /// <summary>
    /// Class NameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class RoleIdValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNameValidator"/> class.
        /// </summary>
        public RoleIdValidator(string? parentName = null, string? propertyName = null)
            : base(parentName, propertyName, false, 1, 22, false)
        {
        }
    }
}
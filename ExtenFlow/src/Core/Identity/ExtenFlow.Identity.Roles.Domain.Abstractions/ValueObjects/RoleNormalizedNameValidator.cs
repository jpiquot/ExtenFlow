using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.Domain.ValueObjects
{
    /// <summary>
    /// Class NormalizedNameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class RoleNormalizedNameValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNormalizedNameValidator"/> class.
        /// </summary>
        public RoleNormalizedNameValidator(string? parentName = null, string? propertyName = null)
            : base(parentName, propertyName, false, 1, 256, false)
        {
        }
    }
}
using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.Domain.ValueObjects
{
    /// <summary>
    /// Class NameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class RoleNameValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNameValidator"/> class.
        /// </summary>
        public RoleNameValidator(string? parent = null, string? property = null)
            : base(parent, property, false, 1, 256, false)
        {
        }
    }
}
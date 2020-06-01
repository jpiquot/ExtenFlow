using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.Domain.ValueObjects
{
    /// <summary>
    /// Class ClaimTypeValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class RoleClaimTypeValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimTypeValidator"/> class.
        /// </summary>
        public RoleClaimTypeValidator(string? parentName = null, string? propertyName = null) : base(parentName, propertyName, false, 1, 256, false)
        {
        }
    }
}
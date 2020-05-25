using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.ValueObjects
{
    /// <summary>
    /// Class ClaimValueValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class RoleClaimValueValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimValueValidator"/> class.
        /// </summary>
        public RoleClaimValueValidator(string? parentName = null, string? propertyName = null)
            : base(propertyName, propertyName, true, 0, int.MaxValue, true)
        {
        }
    }
}
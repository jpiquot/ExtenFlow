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
        /// <param name="parentName">Name of the parent.</param>
        /// <param name="propertyName">Name of the property.</param>
        public RoleClaimValueValidator(string? parentName = null, string? propertyName = null)
            : base(parentName, propertyName, true, 0, int.MaxValue, true)
        {
        }
    }
}
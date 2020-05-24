using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Class ClaimTypeValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class ClaimTypeValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimTypeValidator"/> class.
        /// </summary>
        public ClaimTypeValidator() : base(false, 1, 256, false)
        {
        }
    }
}
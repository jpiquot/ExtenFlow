using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Class ClaimValueValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class ClaimValueValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimValueValidator"/> class.
        /// </summary>
        public ClaimValueValidator() : base(true, 0, int.MaxValue, true)
        {
        }
    }
}
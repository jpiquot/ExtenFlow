using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Class NormalizedNameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class NormalizedNameValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedNameValidator"/> class.
        /// </summary>
        public NormalizedNameValidator() : base(false, 1, 256, false)
        {
        }
    }
}
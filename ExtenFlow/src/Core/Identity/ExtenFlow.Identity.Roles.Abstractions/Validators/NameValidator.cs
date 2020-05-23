using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Class NameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class NameValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameValidator"/> class.
        /// </summary>
        public NameValidator() : base(false, 1, 256, false)
        {
        }
    }
}
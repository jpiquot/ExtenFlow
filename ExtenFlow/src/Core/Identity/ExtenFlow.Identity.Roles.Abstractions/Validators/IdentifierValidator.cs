using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Class NameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class IdentifierValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameValidator"/> class.
        /// </summary>
        public IdentifierValidator() : base(false, 1, 22, false)
        {
        }
    }
}
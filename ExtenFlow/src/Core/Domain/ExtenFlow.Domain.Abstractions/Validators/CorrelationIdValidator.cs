using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Domain.Validators
{
    /// <summary>
    /// Class NameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class CorrelationIdValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationIdValidator"/> class.
        /// </summary>
        public CorrelationIdValidator(string? parentName = null, string? propertyName = null)
            : base(parentName, propertyName, false, 22, 22, false)
        {
        }
    }
}
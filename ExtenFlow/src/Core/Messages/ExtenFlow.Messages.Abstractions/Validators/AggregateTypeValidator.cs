using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Messages
{
    /// <summary>
    /// Class NameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class AggregateTypeValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateTypeValidator"/> class.
        /// </summary>
        public AggregateTypeValidator(string? parentName = null, string? propertyName = null)
            : base(parentName, propertyName, false, 1, 128, false)
        {
        }
    }
}
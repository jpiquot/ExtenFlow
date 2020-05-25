using System.Collections.Generic;

namespace ExtenFlow.Infrastructure.Validators
{
    /// <summary>
    /// Class Validator. Implements the <see cref="ExtenFlow.Infrastructure.IValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.IValidator"/>
    public abstract class PropertyValidator : Validator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValidator"/> class.
        /// </summary>
        protected PropertyValidator(string? parentName, string? propertyName, ValidatorMessageLevel nullErrorLevel) : base($"{parentName}.{propertyName}", nullErrorLevel)
        {
        }

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        public override IList<ValidatorMessage> Validate(object? value) => base.Validate(value);
    }
}
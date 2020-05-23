using System.Collections.Generic;

namespace ExtenFlow.Infrastructure
{
    /// <summary>
    /// Interface IValidator
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Checks the valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="ExtenFlow.Infrastructure.ValueValidationException"></exception>
        void CheckValid(object? value);

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        IList<ValidatorMessage> Validate(object? value);
    }
}
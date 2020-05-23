using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExtenFlow.Infrastructure.Validators
{
    /// <summary>
    /// Class Validator. Implements the <see cref="ExtenFlow.Infrastructure.IValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.IValidator"/>
    public abstract class Validator : IValidator
    {
        private readonly bool _nullable;

        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class.
        /// </summary>
        /// <param name="nullable">if set to <c>true</c> [nullable].</param>
        protected Validator(bool nullable = true)
        {
            _nullable = nullable;
        }

        /// <summary>
        /// Checks the valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="ExtenFlow.Infrastructure.ValueValidationException"></exception>
        public void CheckValid(object? value)
        {
            IList<ValidatorMessage> messages = Validate(value);
            if (messages.Any(p => p.Level == ValidatorMessageLevel.Error))
            {
                throw new ValueValidationException(messages);
            }
        }

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        public virtual IList<ValidatorMessage> Validate(object? value)
        {
            if (value == null && !_nullable)
            {
                return new[] { new ValidatorMessage(ValidatorMessageLevel.Error, Properties.Resources.NullValueNotSupported) };
            }
            return Array.Empty<ValidatorMessage>();
        }

        /// <summary>
        /// Types the mismatch error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>ValidatorMessage.</returns>
        protected static ValidatorMessage TypeMismatchError<T>(object value)
            => new ValidatorMessage(
                ValidatorMessageLevel.Error,
                string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.TypeMismatch,
                    value?.GetType().Name,
                    typeof(string).Name
                ));
    }
}
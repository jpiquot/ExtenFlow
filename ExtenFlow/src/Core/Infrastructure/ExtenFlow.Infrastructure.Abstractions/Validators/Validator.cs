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
        private readonly ValidatorMessageLevel _nullErrorLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class.
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="nullErrorLevel">The null error level.</param>
        protected Validator(string? instanceName, ValidatorMessageLevel nullErrorLevel = ValidatorMessageLevel.Error)
        {
            _nullErrorLevel = nullErrorLevel;
            InstanceName = instanceName;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        protected string? InstanceName { get; }

        /// <summary>
        /// Checks if the object is valid.
        /// </summary>
        /// <param name="value">The object instance.</param>
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
            var messages = new List<ValidatorMessage>();
            if (value == null)
            {
                messages.Add(new ValidatorMessage(_nullErrorLevel, Properties.Resources.ValueIsNull));
            }
            return messages;
        }

        /// <summary>
        /// Creates a type mismatch error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>ValidatorMessage.</returns>
        protected ValidatorMessage TypeMismatchError<T>(object value)
            => new ValidatorMessage(
                ValidatorMessageLevel.Error,
                string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.TypeMismatch,
                    InstanceName,
                    value?.GetType().Name,
                    typeof(string).Name
                ));
    }
}
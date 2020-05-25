using System.Collections.Generic;
using System.Globalization;

namespace ExtenFlow.Infrastructure.Validators
{
    /// <summary>
    /// Class StringValidator.
    /// </summary>
    public abstract class StringValidator : PropertyValidator
    {
        private readonly bool _canBeWhiteSpaces;
        private readonly int _maxLength;
        private readonly int _minLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringValidator"/> class.
        /// </summary>
        protected StringValidator(
            string? parentName,
            string? propertyName,
            bool nullable,
            int minLength,
            int maxLength,
            bool canBeWhiteSpaces)
            : base(parentName, propertyName, nullable ? ValidatorMessageLevel.Information : ValidatorMessageLevel.Error)
        {
            _minLength = minLength;
            _maxLength = maxLength;
            _canBeWhiteSpaces = canBeWhiteSpaces;
        }

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        public override IList<ValidatorMessage> Validate(object? value)
        {
            var list = new List<ValidatorMessage>(base.Validate(value));
            if (value != null)
            {
                if (value is string strValue)
                {
                    if (strValue.Length > 0 && string.IsNullOrWhiteSpace(strValue) && !_canBeWhiteSpaces)
                    {
                        list.Add(new ValidatorMessage(ValidatorMessageLevel.Error, Properties.Resources.WhiteSpacesNotSupported));
                    }
                    if (strValue.Length < _minLength || strValue.Length > _maxLength)
                    {
                        list.Add(new ValidatorMessage(
                            ValidatorMessageLevel.Error,
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resources.StringLengthNotSupported,
                                strValue,
                                strValue.Length,
                                _minLength,
                                _maxLength
                            )));
                    }
                }
                else
                {
                    list.Add(TypeMismatchError<string>(value));
                }
            }
            return list;
        }
    }
}
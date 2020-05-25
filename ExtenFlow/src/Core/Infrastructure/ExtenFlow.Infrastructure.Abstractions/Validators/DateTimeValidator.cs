using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExtenFlow.Infrastructure.Validators
{
    /// <summary>
    /// Class DateTimeValidator.
    /// </summary>
    public abstract class DateTimeValidator : PropertyValidator
    {
        private readonly bool _compareUtc;
        private readonly DateTime? _maxDate;
        private readonly TimeSpan? _maxInTheFuture;
        private readonly TimeSpan? _maxInThePast;
        private readonly DateTime? _minDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeValidator"/> class.
        /// </summary>
        protected DateTimeValidator(
            string? parentName,
            string? propertyName,
            bool nullable,
            bool compareUtc = false,
            TimeSpan? maxInThePast = null,
            TimeSpan? maxInTheFuture = null,
            DateTime? minDate = null,
            DateTime? maxDate = null)
            : base(parentName, propertyName, nullable ? ValidatorMessageLevel.Information : ValidatorMessageLevel.Error)
        {
            _compareUtc = compareUtc;
            _maxInThePast = maxInThePast;
            _maxInTheFuture = maxInTheFuture;
            _minDate = minDate;
            _maxDate = maxDate;
        }

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        public override IList<ValidatorMessage> Validate(object? value)
        {
            var list = new List<ValidatorMessage>(base.Validate(value));
            if (value == null)
            {
                return list;
            }
            DateTime? dateTime = value as DateTime?;
            DateTimeOffset? dateTimeOffset = value as DateTimeOffset?;
            if (dateTimeOffset != null)
            {
                dateTime = (_compareUtc) ? dateTimeOffset.Value.UtcDateTime : dateTimeOffset.Value.DateTime;
            }
            if (dateTime == null)
            {
                list.Add(TypeMismatchError<string>(value));
                return list;
            }
            if (_minDate != null && _minDate > dateTime)
            {
                list.Add(new ValidatorMessage(
                    ValidatorMessageLevel.Error,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.BeforeMininumDateTime,
                        dateTime,
                        _minDate
                    )));
            }

            if (_maxDate != null && _maxDate < dateTime)
            {
                list.Add(new ValidatorMessage(
                    ValidatorMessageLevel.Error,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resources.AfterMaximumDateTime,
                        dateTime,
                        _maxDate
                    )));
            }
            if (_maxInThePast != null)
            {
                DateTimeOffset? maxPast = ((_compareUtc) ? DateTimeOffset.UtcNow : DateTimeOffset.Now) - _maxInThePast;
                if ((dateTimeOffset ?? dateTime) < maxPast)
                {
                    list.Add(new ValidatorMessage(
                        ValidatorMessageLevel.Error,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resources.BeforeMaximumPast,
                            (dateTimeOffset ?? dateTime),
                            _maxInThePast
                        )));
                }
            }

            if (_maxInTheFuture != null)
            {
                DateTimeOffset? maxFuture = ((_compareUtc) ? DateTimeOffset.UtcNow : DateTimeOffset.Now) - _maxInTheFuture;
                if ((dateTimeOffset ?? dateTime) > maxFuture)
                {
                    list.Add(new ValidatorMessage(
                        ValidatorMessageLevel.Error,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resources.AfterMaximumFuture,
                            (dateTimeOffset ?? dateTime),
                            _maxInTheFuture
                        )));
                }
            }
            return list;
        }
    }
}
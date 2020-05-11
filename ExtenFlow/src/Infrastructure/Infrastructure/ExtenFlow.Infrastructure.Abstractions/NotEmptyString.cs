using System;
using System.Collections.Generic;

namespace ExtenFlow.Infrastructure
{
    /// <summary>
    /// Class NotEmptyString. Implements the <see cref="ExtenFlow.Infrastructure.ValueObject"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.ValueObject"/>
    public class NotEmptyString : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotEmptyString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public NotEmptyString(string value)
        {
            Result result = Validate(value);
            if (result.HasFailed)
            {
                throw new ArgumentException(string.Join("\n", result.Messages));
            }
            Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        protected string Value { get; }

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;System.String&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        protected static Result Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failed(Properties.Resources.EmptyStringValueNotSupported);
            }
            return Result.Succeeded();
        }

        /// <summary>
        /// Gets the equality components.
        /// </summary>
        /// <returns>IEnumerable&lt;System.Object&gt;.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
using System;
using System.Collections.Generic;

#pragma warning disable CS8601 // Possible null reference assignment.

namespace ExtenFlow.Infrastructure
{
    /// <summary>
    /// Value object
    /// </summary>
    public abstract class ValueObject<TValue> : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObject{TValue}"/> class.
        /// </summary>
        protected ValueObject(TValue value, IValidator validator)
        {
            _ = validator ?? throw new ArgumentNullException(nameof(validator));
            validator.CheckValid(value);
            Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public TValue Value { get; }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        public override int GetHashCode() => (Value == null) ? 0 : Value.GetHashCode();

        /// <summary>
        /// Gets the atomic values.
        /// </summary>
        /// <returns>IEnumerable&lt;System.Object&gt;.</returns>
        protected override IEnumerable<object> GetAtomicValues() => new object[] { Value };
    }
}
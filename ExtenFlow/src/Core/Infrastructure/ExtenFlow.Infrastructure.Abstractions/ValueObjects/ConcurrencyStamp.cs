using System;

using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Infrastructure.ValueObjects
{
    /// <summary>
    /// Class ConcurrencyStamp.
    /// </summary>
    public class ConcurrencyStamp : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyStamp"/> class.
        /// </summary>
        /// <param name="concurrencyStamp">The concurrency stamp.</param>
        public ConcurrencyStamp(string concurrencyStamp) : base(concurrencyStamp, new ConcurrencyStampValidator())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyStamp"/> class.
        /// </summary>
        public ConcurrencyStamp() : this(Guid.NewGuid().ToBase64String())
        {
        }
    }
}
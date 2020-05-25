using System;

using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Infrastructure.ValueObjects
{
    /// <summary>
    /// Class ConcurrencyStamp.
    /// </summary>
    public class ConcurrencyCheckStamp : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyCheckStamp"/> class.
        /// </summary>
        /// <param name="concurrencyStamp">The concurrency stamp.</param>
        public ConcurrencyCheckStamp(string concurrencyStamp) : base(concurrencyStamp, new ConcurrencyStampValidator())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyCheckStamp"/> class.
        /// </summary>
        public ConcurrencyCheckStamp() : this(Guid.NewGuid().ToBase64String())
        {
        }
    }
}
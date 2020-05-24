using ExtenFlow.Identity.Roles.Validators;
using ExtenFlow.Infrastructure;

namespace ExtenFlow.Identity.Roles.ValueObjects
{
    /// <summary>
    /// Class ClaimTypeValue
    /// </summary>
    public class ClaimType : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimType"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ClaimType(string value) : base(value, new ClaimTypeValidator())
        {
        }
    }
}
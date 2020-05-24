using ExtenFlow.Identity.Roles.Validators;
using ExtenFlow.Infrastructure;

namespace ExtenFlow.Identity.Roles.ValueObjects
{
    /// <summary>
    /// Class ClaimValueValue
    /// </summary>
    public class ClaimValue : ValueObject<string?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ClaimValue(string value) : base(value, new ClaimValueValidator())
        {
        }
    }
}
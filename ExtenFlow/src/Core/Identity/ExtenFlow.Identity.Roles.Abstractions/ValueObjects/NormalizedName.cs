using ExtenFlow.Identity.Roles.Validators;
using ExtenFlow.Infrastructure;

namespace ExtenFlow.Identity.Roles.ValueObjects
{
    /// <summary>
    /// Class NormalizedNameValue
    /// </summary>
    public class NormalizedName : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalizedName"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public NormalizedName(string value) : base(value, new NameValidator())
        {
        }
    }
}
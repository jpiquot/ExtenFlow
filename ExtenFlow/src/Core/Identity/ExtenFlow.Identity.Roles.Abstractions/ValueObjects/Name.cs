using ExtenFlow.Identity.Roles.Validators;
using ExtenFlow.Infrastructure;

namespace ExtenFlow.Identity.Roles.ValueObjects
{
    /// <summary>
    /// Class NameValue
    /// </summary>
    public class Name : ValueObject<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Name"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Name(string value) : base(value, new NameValidator())
        {
        }
    }
}
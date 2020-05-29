using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Messages
{
    /// <summary>
    /// Class NameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.StringValidator"/>
    public class MessageIdValidator : StringValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageIdValidator"/> class.
        /// </summary>
        public MessageIdValidator(string? parentName = null, string? propertyName = null)
            : base(parentName, propertyName, false, 22, 22, false)
        {
        }
    }
}
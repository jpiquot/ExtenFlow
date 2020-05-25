using ExtenFlow.Infrastructure.Validators;

namespace ExtenFlow.Domain.Validators
{
    /// <summary>
    /// Class NameValidator. Implements the <see cref="ExtenFlow.Infrastructure.Validators.DateTimeValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.Validators.DateTimeValidator"/>
    public class MessageDateTimeValidator : DateTimeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDateTimeValidator"/> class.
        /// </summary>
        public MessageDateTimeValidator(string? parentName = null, string? propertyName = null)
            : base(parentName,
                   propertyName,
                   false,
                   true,
                   null,
                   new System.TimeSpan(0, 1, 0),
                   new System.DateTime(2020, 1, 1),
                   new System.DateTime(2050, 1, 1))
        {
        }
    }
}
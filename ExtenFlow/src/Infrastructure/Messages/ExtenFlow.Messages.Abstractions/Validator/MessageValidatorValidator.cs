using FluentValidation;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Messages.Validators
{
    /// <summary>
    /// Message validation
    /// </summary>
    public abstract class MessageValidator<T> : AbstractValidator<T> where T : IMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected MessageValidator(bool aggregateIdRequired = true)
        {
            RuleFor(message => message.UserId).NotEmpty().WithMessage(Properties.Resources.UserIdNotDefined);
            RuleFor(message => message.MessageId).NotEmpty().WithMessage(Properties.Resources.MessageIdNotDefined);
            RuleFor(message => message.CorrelationId).NotEmpty().WithMessage(Properties.Resources.CorrelationIdNotDefined);
            RuleFor(message => message.AggregateType).NotEmpty().WithMessage(Properties.Resources.AggregateTypeNotDefined);
            RuleFor(message => message.DateTime).NotEmpty().WithMessage(Properties.Resources.MessageDateTimeNotDefined);
            if (aggregateIdRequired)
            {
                RuleFor(message => message.AggregateId).NotEmpty().WithMessage(Properties.Resources.AggregateIdNotDefined);
            }
        }
    }
}
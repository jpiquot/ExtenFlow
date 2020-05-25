using System.Collections.Generic;

using ExtenFlow.Infrastructure;
using ExtenFlow.Infrastructure.Validators;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Domain.Validators
{
    /// <summary>
    /// Message validation
    /// </summary>
    public abstract class MessageValidator : Validator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected MessageValidator(string? instanceName) : base(instanceName, ValidatorMessageLevel.Error)
        {
        }

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        public override IList<ValidatorMessage> Validate(object? value)
        {
            var messages = new List<ValidatorMessage>(base.Validate(value));
            if (value != null)
            {
                if (value is IMessage message)
                {
                    messages.AddRange(new MessageIdValidator(InstanceName, nameof(IMessage.Id)).Validate(message.Id));
                    messages.AddRange(new CorrelationIdValidator(InstanceName, nameof(IMessage.CorrelationId)).Validate(message.CorrelationId));
                    messages.AddRange(new AggregateIdValidator(InstanceName, nameof(IMessage.AggregateId)).Validate(message.AggregateId));
                    messages.AddRange(new AggregateTypeValidator(InstanceName, nameof(IMessage.AggregateType)).Validate(message.AggregateType));
                    messages.AddRange(new MessageDateTimeValidator(InstanceName, nameof(IMessage.DateTime)).Validate(message.DateTime));
                    messages.AddRange(new MessageUserIdValidator(InstanceName, nameof(IMessage.UserId)).Validate(message.UserId));
                    messages.AddRange(ValidateMessage(message));
                }
                else
                {
                    messages.Add(TypeMismatchError<IMessage>(value));
                }
            }
            return messages;
        }

        /// <summary>
        /// Validates the message.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected virtual IList<ValidatorMessage> ValidateMessage(IMessage value) => new List<ValidatorMessage>();
    }
}
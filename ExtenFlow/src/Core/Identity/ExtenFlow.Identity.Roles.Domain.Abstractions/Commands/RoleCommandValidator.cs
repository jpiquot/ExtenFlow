using System.Collections.Generic;
using System.Globalization;

using ExtenFlow.Identity.Roles.Domain.ValueObjects;
using ExtenFlow.Infrastructure;
using ExtenFlow.Messages;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Domain.Commands
{
    /// <summary>
    /// Role command validation
    /// </summary>
    public abstract class RoleCommandValidator : CommandValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleCommandValidator"/> class.
        /// </summary>
        /// <param name="instanceName">The instance.</param>
        protected RoleCommandValidator(string? instanceName) : base(instanceName)
        {
        }

        /// <summary>
        /// Validates the command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateCommand(ICommand value)
        {
            var messages = new List<ValidatorMessage>();
            if (value is RoleCommand command)
            {
                messages.AddRange(new RoleIdValidator(InstanceName, nameof(IMessage.AggregateId)).Validate(command.AggregateId));
                if (command.AggregateType != AggregateName.Role)
                {
                    messages.Add(new ValidatorMessage(
                        ValidatorMessageLevel.Error,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            ExtenFlow.Domain.Properties.Resources.AggregateTypeMismatch,
                            AggregateName.Role,
                            command.AggregateType
                            )));
                }
                messages.AddRange(ValidateRoleCommand(command));
            }
            else
            {
                messages.Add(TypeMismatchError<RoleCommand>(value));
            }
            return messages;
        }

        /// <summary>
        /// Validates the role command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected virtual IList<ValidatorMessage> ValidateRoleCommand(RoleCommand command)
            => new List<ValidatorMessage>();
    }
}
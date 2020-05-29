using System.Collections.Generic;
using System.Globalization;

using ExtenFlow.Identity.Roles.ValueObjects;
using ExtenFlow.Infrastructure;
using ExtenFlow.Messages;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Commands
{
    /// <summary>
    /// Role command validation
    /// </summary>
    public abstract class RoleNameRegistryCommandValidator : CommandValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNameRegistryCommandValidator"/> class.
        /// </summary>
        /// <param name="instanceName">The instance.</param>
        protected RoleNameRegistryCommandValidator(string? instanceName) : base(instanceName)
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
            if (value is RoleNameRegistryCommand command)
            {
                messages.AddRange(new RoleNormalizedNameValidator(InstanceName, nameof(IMessage.AggregateId)).Validate(command.AggregateId));
                if (command.AggregateType != AggregateName.RoleNameRegistry)
                {
                    messages.Add(new ValidatorMessage(
                        ValidatorMessageLevel.Error,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Domain.Properties.Resources.AggregateTypeMismatch,
                            AggregateName.Role,
                            command.AggregateType
                            )));
                }
                messages.AddRange(ValidateRoleNameRegistryCommand(command));
            }
            else
            {
                messages.Add(TypeMismatchError<RoleNameRegistryCommand>(value));
            }
            return messages;
        }

        /// <summary>
        /// Validates the role name registry command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected virtual IList<ValidatorMessage> ValidateRoleNameRegistryCommand(RoleNameRegistryCommand command)
            => new List<ValidatorMessage>();
    }
}
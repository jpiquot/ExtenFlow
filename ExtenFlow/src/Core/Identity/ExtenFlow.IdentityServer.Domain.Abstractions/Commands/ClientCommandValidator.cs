using System.Collections.Generic;
using System.Globalization;

using ExtenFlow.IdentityServer.Domain.ValueObjects;
using ExtenFlow.Infrastructure;
using ExtenFlow.Messages;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.IdentityServer.Domain.Commands
{
    /// <summary>
    /// Client command validation
    /// </summary>
    public abstract class ClientCommandValidator : CommandValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCommandValidator"/> class.
        /// </summary>
        /// <param name="instanceName">The instance.</param>
        protected ClientCommandValidator(string? instanceName) : base(instanceName)
        {
        }

        /// <summary>
        /// Validates the client command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected virtual IList<ValidatorMessage> ValidateClientCommand(ClientCommand command)
            => new List<ValidatorMessage>();

        /// <summary>
        /// Validates the command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateCommand(ICommand value)
        {
            var messages = new List<ValidatorMessage>();
            if (value is ClientCommand command)
            {
                messages.AddRange(new ClientIdValidator(InstanceName, nameof(IMessage.AggregateId)).Validate(command.AggregateId));
                if (command.AggregateType != AggregateName.Client)
                {
                    messages.Add(new ValidatorMessage(
                        ValidatorMessageLevel.Error,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            ExtenFlow.Domain.Properties.Resources.AggregateTypeMismatch,
                            AggregateName.Client,
                            command.AggregateType
                            )));
                }
                messages.AddRange(ValidateClientCommand(command));
            }
            else
            {
                messages.Add(TypeMismatchError<ClientCommand>(value));
            }
            return messages;
        }
    }
}
using System.Collections.Generic;

using ExtenFlow.IdentityServer.Domain.ValueObjects;
using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.IdentityServer.Domain.Commands
{
    /// <summary>
    /// Add new client command validation
    /// </summary>
    public class AddNewClientValidator : ClientCommandValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewClientValidator"/> class.
        /// </summary>
        public AddNewClientValidator() : base(nameof(AddNewClient))
        {
        }

        /// <summary>
        /// Validates the client command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateClientCommand(ClientCommand value)
        {
            var messages = new List<ValidatorMessage>();
            if (value is AddNewClient command)
            {
                messages.AddRange(new ClientNameValidator(InstanceName, nameof(AddNewClient.Name)).Validate(command.Name));
                messages.AddRange(new ClientNormalizedNameValidator(InstanceName, nameof(AddNewClient.Description)).Validate(command.Description));
            }
            else
            {
                messages.Add(TypeMismatchError<AddNewClient>(value));
            }
            return messages;
        }
    }
}
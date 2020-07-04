using System.Collections.Generic;

using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.IdentityServer.Domain.Commands
{
    /// <summary>
    /// Remove client command validation
    /// </summary>
    public class RemoveClientValidator : ClientCommandValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RemoveClientValidator() : base(nameof(RemoveClient))
        {
        }

        /// <summary>
        /// Validates the add client claim command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateClientCommand(ClientCommand value)
        {
            var messages = new List<ValidatorMessage>();
            if (!(value is RemoveClient))
            {
                messages.Add(TypeMismatchError<RemoveClient>(value));
            }
            return messages;
        }
    }
}
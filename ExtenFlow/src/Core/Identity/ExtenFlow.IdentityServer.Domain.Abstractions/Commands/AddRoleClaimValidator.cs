using System.Collections.Generic;

using ExtenFlow.IdentityServer.Domain.ValueObjects;
using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.IdentityServer.Domain.Commands
{
    /// <summary>
    /// Add new client claim command validation
    /// </summary>
    public class AddClientClaimValidator : ClientCommandValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddClientClaimValidator"/> class.
        /// </summary>
        public AddClientClaimValidator() : base(nameof(AddClientClaim))
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
            if (value is AddClientClaim command)
            {
                messages.AddRange(new ClientClaimTypeValidator(InstanceName, nameof(AddClientClaim.ClaimType)).Validate(command.ClaimType));
                messages.AddRange(new ClientClaimValueValidator(InstanceName, nameof(AddClientClaim.ClaimValue)).Validate(command.ClaimValue));
            }
            else
            {
                messages.Add(TypeMismatchError<AddClientClaim>(value));
            }
            return messages;
        }
    }
}
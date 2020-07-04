using System.Collections.Generic;

using ExtenFlow.IdentityServer.Domain.Commands;
using ExtenFlow.IdentityServer.Domain.ValueObjects;
using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.IdentityServer.Domain.Commands
{
    /// <summary>
    /// Remove client claim command validation
    /// </summary>
    public sealed class RemoveClientClaimValidator : ClientCommandValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RemoveClientClaimValidator() : base(nameof(RemoveClientClaim))
        {
        }

        /// <summary>
        /// Validates the client command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Collections.Generic.IList&lt;ExtenFlow.Infrastructure.ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateClientCommand(ClientCommand value)
        {
            var messages = new List<ValidatorMessage>();
            if (value is RemoveClientClaim command)
            {
                messages.AddRange(new ClientClaimTypeValidator(InstanceName, nameof(RemoveClientClaim.ClaimType)).Validate(command.ClaimType));
                messages.AddRange(new ClientClaimValueValidator(InstanceName, nameof(RemoveClientClaim.ClaimValue)).Validate(command.ClaimValue));
            }
            else
            {
                messages.Add(TypeMismatchError<RemoveClientClaim>(value));
            }
            return messages;
        }
    }
}
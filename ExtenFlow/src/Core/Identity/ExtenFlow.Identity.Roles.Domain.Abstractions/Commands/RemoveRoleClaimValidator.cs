using System.Collections.Generic;

using ExtenFlow.Identity.Roles.Domain.Commands;
using ExtenFlow.Identity.Roles.Domain.ValueObjects;
using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Domain.Commands
{
    /// <summary>
    /// Remove role claim command validation
    /// </summary>
    public sealed class RemoveRoleClaimValidator : RoleCommandValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RemoveRoleClaimValidator() : base(nameof(RemoveRoleClaim))
        {
        }

        /// <summary>
        /// Validates the role command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Collections.Generic.IList&lt;ExtenFlow.Infrastructure.ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRoleCommand(RoleCommand value)
        {
            var messages = new List<ValidatorMessage>();
            if (value is RemoveRoleClaim command)
            {
                messages.AddRange(new RoleClaimTypeValidator(InstanceName, nameof(RemoveRoleClaim.ClaimType)).Validate(command.ClaimType));
                messages.AddRange(new RoleClaimValueValidator(InstanceName, nameof(RemoveRoleClaim.ClaimValue)).Validate(command.ClaimValue));
            }
            else
            {
                messages.Add(TypeMismatchError<RemoveRoleClaim>(value));
            }
            return messages;
        }
    }
}
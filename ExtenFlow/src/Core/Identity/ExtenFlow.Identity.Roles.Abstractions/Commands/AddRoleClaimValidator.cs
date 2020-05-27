using System.Collections.Generic;

using ExtenFlow.Identity.Roles.ValueObjects;
using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Commands
{
    /// <summary>
    /// Add new role claim command validation
    /// </summary>
    public class AddRoleClaimValidator : RoleCommandValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddRoleClaimValidator"/> class.
        /// </summary>
        public AddRoleClaimValidator() : base(nameof(AddRoleClaim))
        {
        }

        /// <summary>
        /// Validates the add role claim command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRoleCommand(RoleCommand value)
        {
            var messages = new List<ValidatorMessage>();
            if (value is AddRoleClaim command)
            {
                messages.AddRange(new RoleClaimTypeValidator(InstanceName, nameof(AddRoleClaim.ClaimType)).Validate(command.ClaimType));
                messages.AddRange(new RoleClaimValueValidator(InstanceName, nameof(AddRoleClaim.ClaimValue)).Validate(command.ClaimValue));
            }
            else
            {
                messages.Add(TypeMismatchError<AddRoleClaim>(value));
            }
            return messages;
        }
    }
}
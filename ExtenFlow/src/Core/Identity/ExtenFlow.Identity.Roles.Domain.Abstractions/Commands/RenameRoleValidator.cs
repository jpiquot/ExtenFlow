using System.Collections.Generic;

using ExtenFlow.Identity.Roles.Domain.Commands;
using ExtenFlow.Identity.Roles.Domain.ValueObjects;
using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Domain.Commands
{
    /// <summary>
    /// Rename role command validation
    /// </summary>
    public class RenameRoleValidator : RoleCommandValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameRoleValidator"/> class.
        /// </summary>
        public RenameRoleValidator() : base(nameof(RenameRole))
        {
        }

        /// <summary>
        /// Validates the role command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRoleCommand(RoleCommand value)
        {
            var messages = new List<ValidatorMessage>();
            if (value is RenameRole command)
            {
                messages.AddRange(new RoleNameValidator(InstanceName, nameof(RenameRole.Name)).Validate(command.Name));
                messages.AddRange(new RoleNormalizedNameValidator(InstanceName, nameof(RenameRole.NormalizedName)).Validate(command.NormalizedName));
            }
            else
            {
                messages.Add(TypeMismatchError<RenameRole>(value));
            }
            return messages;
        }
    }
}
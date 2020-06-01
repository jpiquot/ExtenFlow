using System.Collections.Generic;

using ExtenFlow.Identity.Roles.Domain.ValueObjects;
using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Domain.Commands
{
    /// <summary>
    /// Add new role command validation
    /// </summary>
    public class AddNewRoleValidator : RoleCommandValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewRoleValidator"/> class.
        /// </summary>
        public AddNewRoleValidator() : base(nameof(AddNewRole))
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
            if (value is AddNewRole command)
            {
                messages.AddRange(new RoleNameValidator(InstanceName, nameof(AddNewRole.Name)).Validate(command.Name));
                messages.AddRange(new RoleNormalizedNameValidator(InstanceName, nameof(AddNewRole.NormalizedName)).Validate(command.NormalizedName));
            }
            else
            {
                messages.Add(TypeMismatchError<AddNewRole>(value));
            }
            return messages;
        }
    }
}
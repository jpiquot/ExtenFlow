using System.Collections.Generic;

using ExtenFlow.Identity.Roles.Domain.ValueObjects;
using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Domain.Commands
{
    /// <summary>
    /// Add new role claim command validation
    /// </summary>
    public class AddUserToRoleValidator : RoleCommandValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserToRoleValidator"/> class.
        /// </summary>
        public AddUserToRoleValidator() : base(nameof(AddRoleClaim))
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
            if (value is AddUserToRole command)
            {
                messages.AddRange(new RoleUserIdValidator(InstanceName, nameof(AddUserToRole.RoleUserId)).Validate(command.RoleUserId));
            }
            else
            {
                messages.Add(TypeMismatchError<AddUserToRole>(value));
            }
            return messages;
        }
    }
}
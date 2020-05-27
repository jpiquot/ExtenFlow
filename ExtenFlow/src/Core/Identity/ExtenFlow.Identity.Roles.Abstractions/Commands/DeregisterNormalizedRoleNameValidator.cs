using System.Collections.Generic;

using ExtenFlow.Identity.Roles.ValueObjects;
using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Commands
{
    /// <summary>
    /// Class DeregisterNormalizedRoleNameValidator. Implements the <see cref="ExtenFlow.Identity.Roles.Commands.RoleCommandValidator"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Roles.Commands.RoleCommandValidator"/>
    public class DeregisterNormalizedRoleNameValidator : RoleNameRegistryCommandValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeregisterNormalizedRoleNameValidator"/> class.
        /// </summary>
        public DeregisterNormalizedRoleNameValidator() : base(nameof(AddRoleClaim))
        {
        }

        /// <summary>
        /// Validates the role command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRoleNameRegistryCommand(RoleNameRegistryCommand value)
        {
            var messages = new List<ValidatorMessage>();
            if (value is DeregisterNormalizedRoleName command)
            {
                messages.AddRange(new RoleIdValidator(InstanceName, nameof(DeregisterNormalizedRoleName.RoleId)).Validate(command.RoleId));
            }
            else
            {
                messages.Add(TypeMismatchError<DeregisterNormalizedRoleName>(value));
            }
            return messages;
        }
    }
}
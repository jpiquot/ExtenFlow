using System.Collections.Generic;

using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Domain.Commands
{
    /// <summary>
    /// Remove role command validation
    /// </summary>
    public class RemoveRoleValidator : RoleCommandValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RemoveRoleValidator() : base(nameof(RemoveRole))
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
            if (!(value is RemoveRole))
            {
                messages.Add(TypeMismatchError<RemoveRole>(value));
            }
            return messages;
        }
    }
}
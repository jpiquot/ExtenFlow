using System.Collections.Generic;

using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Queries
{
    /// <summary>
    /// Add new role query validation
    /// </summary>
    public class IsRoleNameRegisteredValidator : RoleNameRegistryQueryValidator<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsRoleNameRegisteredValidator"/> class.
        /// </summary>
        public IsRoleNameRegisteredValidator() : base(nameof(GetRoleIdByName))
        {
        }

        /// <summary>
        /// Validates the role query.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRoleNameRegistryQuery(RoleNameRegistryQuery<bool> value)
        {
            var messages = new List<ValidatorMessage>();
            if (!(value is IsRoleNameRegistered))
            {
                messages.Add(TypeMismatchError<IsRoleNameRegistered>(value));
            }
            return messages;
        }
    }
}
using System.Collections.Generic;

using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Queries
{
    /// <summary>
    /// Class FindRoleIdByNameValidator. Implements the <see cref="ExtenFlow.Identity.Roles.Queries.RoleQueryValidator{T}"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Roles.Queries.RoleQueryValidator{T}"/>
    public class FindRoleIdByNameValidator : RoleNameRegistryQueryValidator<string?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindRoleIdByNameValidator"/> class.
        /// </summary>
        public FindRoleIdByNameValidator() : base(nameof(GetRoleClaims))
        {
        }

        /// <summary>
        /// Validates the role name registry query.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRoleNameRegistryQuery(RoleNameRegistryQuery<string?> value)
        {
            var messages = new List<ValidatorMessage>();
            if (!(value is FindRoleIdByName))
            {
                messages.Add(TypeMismatchError<FindRoleIdByName>(value));
            }
            return messages;
        }
    }
}
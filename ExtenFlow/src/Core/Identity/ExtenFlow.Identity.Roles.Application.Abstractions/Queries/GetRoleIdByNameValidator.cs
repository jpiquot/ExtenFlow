using System.Collections.Generic;

using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Application.Queries
{
    /// <summary>
    /// Add new role query validation
    /// </summary>
    public class GetRoleIdByNameValidator : RoleNameRegistryQueryValidator<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleIdByNameValidator"/> class.
        /// </summary>
        public GetRoleIdByNameValidator() : base(nameof(GetRoleIdByName))
        {
        }

        /// <summary>
        /// Validates the role query.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRoleNameRegistryQuery(RoleNameRegistryQuery<string> value)
        {
            var messages = new List<ValidatorMessage>();
            if (!(value is GetRoleIdByName))
            {
                messages.Add(TypeMismatchError<GetRoleIdByName>(value));
            }
            return messages;
        }
    }
}
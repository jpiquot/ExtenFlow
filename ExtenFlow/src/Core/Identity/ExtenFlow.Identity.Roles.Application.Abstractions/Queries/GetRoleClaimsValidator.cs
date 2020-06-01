using System.Collections.Generic;
using System.Security.Claims;

using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Application.Queries
{
    /// <summary>
    /// Add new role query validation
    /// </summary>
    public class GetRoleClaimsValidator : RoleQueryValidator<IList<Claim>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleClaimsValidator"/> class.
        /// </summary>
        public GetRoleClaimsValidator() : base(nameof(GetRoleClaims))
        {
        }

        /// <summary>
        /// Validates the role query.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRoleQuery(RoleQuery<IList<Claim>> value)
        {
            var messages = new List<ValidatorMessage>();
            if (!(value is GetRoleClaims))
            {
                messages.Add(TypeMismatchError<GetRoleClaims>(value));
            }
            return messages;
        }
    }
}
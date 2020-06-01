using System.Collections.Generic;

using ExtenFlow.Infrastructure;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Application.Queries
{
    /// <summary>
    /// Add new role query validation
    /// </summary>
    public class GetRoleDetailsValidator : RoleQueryValidator<RoleDetails>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleDetailsValidator"/> class.
        /// </summary>
        public GetRoleDetailsValidator() : base(nameof(GetRoleDetails))
        {
        }

        /// <summary>
        /// Validates the role query.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRoleQuery(RoleQuery<RoleDetails> value)
        {
            var messages = new List<ValidatorMessage>();
            if (!(value is GetRoleDetails))
            {
                messages.Add(TypeMismatchError<GetRoleDetails>(value));
            }
            return messages;
        }
    }
}
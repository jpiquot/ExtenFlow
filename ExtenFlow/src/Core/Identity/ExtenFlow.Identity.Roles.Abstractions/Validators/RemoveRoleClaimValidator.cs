using ExtenFlow.Identity.Roles.Commands;

using FluentValidation;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Remove role claim command validation
    /// </summary>
    public class RemoveRoleClaimValidator : RoleCommandValidator<RemoveRoleClaim>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RemoveRoleClaimValidator() : base(true)
        {
            RuleFor(command => command.ClaimType).NotEmpty().WithMessage(Properties.Resources.RoleClaimTypeNotDefined);
        }
    }
}
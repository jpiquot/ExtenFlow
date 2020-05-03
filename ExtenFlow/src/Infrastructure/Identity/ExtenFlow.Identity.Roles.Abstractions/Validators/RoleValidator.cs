using ExtenFlow.Identity.Models;

using FluentValidation;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Role validation
    /// </summary>
    public class RoleValidator : AbstractValidator<Role>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RoleValidator(bool checkConcurrencyStamp = true)
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage(Properties.Resources.RoleIdNotDefined);
            RuleFor(command => command.Name).NotEmpty().WithMessage(Properties.Resources.RoleNameNotDefined);
            RuleFor(command => command.NormalizedName).NotEmpty().WithMessage(Properties.Resources.NormalizedRoleNameNotDefined);
            if (checkConcurrencyStamp)
            {
                RuleFor(command => command.ConcurrencyStamp).NotEmpty().WithMessage(Properties.Resources.RoleConcurrencyStampNotDefined); ;
            }
        }
    }
}
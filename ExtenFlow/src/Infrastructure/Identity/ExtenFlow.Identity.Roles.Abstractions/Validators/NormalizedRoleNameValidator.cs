using FluentValidation.Validators;

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Normalized role name validator
    /// </summary>
    public class NormalizedRoleNameValidator : PropertyValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NormalizedRoleNameValidator() : base(Properties.Resources.RoleNameInvalid)
        {
        }

        /// <summary>
        /// Check if property is valid
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool IsValid(PropertyValidatorContext context)
            => string.IsNullOrWhiteSpace(context?.Instance as string);
    }
}
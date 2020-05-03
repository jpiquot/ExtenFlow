using FluentValidation.Validators;

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Role name validator
    /// </summary>
    public class RoleNameValidator : PropertyValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RoleNameValidator() : base(Properties.Resources.RoleNameInvalid)
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
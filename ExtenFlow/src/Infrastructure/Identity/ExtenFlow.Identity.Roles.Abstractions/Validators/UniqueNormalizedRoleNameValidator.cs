using System;

using FluentValidation.Validators;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Role normalized name uniqueness validation
    /// </summary>
    public class UniqueNormalizedRoleNameValidator : PropertyValidator
    {
        private readonly Func<string, bool> _normalizedNameExists;

        /// <summary>
        /// Constructor
        /// </summary>
        public UniqueNormalizedRoleNameValidator(Func<string, bool> normalizedNameExists) : base(Properties.Resources.DuplicateNormalizedRoleName)
        {
            _normalizedNameExists = normalizedNameExists;
        }

        /// <summary>
        /// Check if property is valid
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            string? name = context?.PropertyValue as string;
            if (string.IsNullOrWhiteSpace(name))
            {
                return true;
            }
            return !_normalizedNameExists(name);
        }
    }
}
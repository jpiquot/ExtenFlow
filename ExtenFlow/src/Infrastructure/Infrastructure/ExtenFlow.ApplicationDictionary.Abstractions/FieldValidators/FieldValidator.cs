using System;
using System.Resources;
using ExtenFlow.ApplicationDictionary.FieldDefinitions;

using FluentValidation.Validators;

[assembly: NeutralResourcesLanguage("en")]

namespace ExtenFlow.ApplicationDictionary.FieldValidators
{
    /// <summary>
    /// Field validator
    /// </summary>
    public abstract class FieldValidator<TField, TType> : PropertyValidator
        where TField : FieldDefinition<TType>, new()
    {
        private TField? _definition;

        /// <summary>
        /// Constructor
        /// </summary>
        public FieldValidator(string? errorMessage = null) : base(errorMessage ?? Properties.Resources.InvalidFieldValue)
        {
        }

        /// <summary>
        /// Field definition
        /// </summary>
        protected TField Definition => _definition ?? (_definition = new TField());

        /// <summary>
        /// Check if property is valid
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            bool isValid = true;
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.PropertyValue == null && !Definition.CanBeNull)
            {
                context.MessageFormatter.AppendArgument(nameof(Definition.CanBeNull), Definition.CanBeNull);
                isValid = false;
            }
            return isValid;
        }
    }
}
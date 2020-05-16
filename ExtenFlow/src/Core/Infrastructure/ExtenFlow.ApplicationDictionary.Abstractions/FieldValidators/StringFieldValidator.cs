using System;

using ExtenFlow.ApplicationDictionary.FieldDefinitions;

using FluentValidation.Validators;

namespace ExtenFlow.ApplicationDictionary.FieldValidators
{
    /// <summary>
    /// String field validator
    /// </summary>
    public abstract class StringFieldValidator<TField> : FieldValidator<TField, string>
        where TField : IStringFieldDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StringFieldValidator(TField definition, string? errorMessage = null) : base(definition, errorMessage)
        {
        }

        /// <summary>
        /// Check if property is valid
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override bool IsValid(PropertyValidatorContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            bool isValid = base.IsValid(context);
            string? value = context.PropertyValue as string;
            if (string.IsNullOrEmpty(value) && !Definition.CanBeEmpty)
            {
                context.MessageFormatter.AppendArgument(nameof(Definition.CanBeEmpty), Definition.CanBeEmpty);
                isValid = false;
            }
            else if (string.IsNullOrWhiteSpace(value) && !Definition.CanBeWhiteSpaces)
            {
                context.MessageFormatter.AppendArgument(nameof(Definition.CanBeWhiteSpaces), Definition.CanBeWhiteSpaces);
                isValid = false;
            }
            if (value?.Length < Definition.Min)
            {
                context.MessageFormatter.AppendArgument(nameof(Definition.Min), Definition.Min);
                isValid = false;
            }
            if (value?.Length > Definition.Max)
            {
                context.MessageFormatter.AppendArgument(nameof(Definition.Max), Definition.Max);
                isValid = false;
            }
            return isValid;
        }
    }
}
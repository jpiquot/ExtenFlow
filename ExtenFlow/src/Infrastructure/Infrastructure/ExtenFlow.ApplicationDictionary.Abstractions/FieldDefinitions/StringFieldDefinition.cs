using System;

namespace ExtenFlow.ApplicationDictionary.FieldDefinitions
{
    /// <summary>
    /// String field/property definition
    /// </summary>
    public abstract class StringFieldDefinition : FieldDefinition<string>
    {
        private readonly bool? _canBeWhiteSpaces;

        /// <summary>
        /// Constructor
        /// </summary>
        protected StringFieldDefinition(
            string name,
            string? displayName,
            string? shortDisplayName,
            string? description,
            string? parentName,
            int? min,
            int? max,
            bool? canBeNull,
            bool? canBeEmpty,
            bool? canBeWhiteSpaces,
            bool? readOnly,
            FieldAlignment? alignment,
            Func<string, StringFieldDefinition> getFieldDefinition)
            : base(name, displayName, shortDisplayName, description, parentName, min, max,
                  canBeNull, canBeEmpty, readOnly, alignment, getFieldDefinition)
        {
            _canBeWhiteSpaces = canBeWhiteSpaces;
            if (Min > 0 && CanBeNull == true)
            {
                throw new ArgumentException(Properties.Resources.CanBeNullAndMinInvalid, nameof(canBeNull));
            }
            if (Min > 0 && CanBeEmpty == true)
            {
                throw new ArgumentException(Properties.Resources.CanBeEmptyAndMinInvalid, nameof(canBeEmpty));
            }
        }

        /// <summary>
        /// The string can be empty
        /// </summary>
        public bool CanBeWhiteSpaces => _canBeWhiteSpaces ?? ((StringFieldDefinition?)Parent)?._canBeWhiteSpaces ?? false;
    }
}
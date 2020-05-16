namespace ExtenFlow.ApplicationDictionary.FieldDefinitions
{
    /// <summary>
    /// Field alignment
    /// </summary>
    public enum FieldAlignment
    {
        /// <summary>
        /// Align on left
        /// </summary>
        Left = 0,

        /// <summary>
        /// Align on right
        /// </summary>
        Right = 1,

        /// <summary>
        /// Align on center
        /// </summary>
        Center = 2
    }

    /// <summary>
    /// String field/property definition interface
    /// </summary>
    public interface IFieldDefinition
    {
        /// <summary>
        /// The value is editable.
        /// </summary>
        FieldAlignment Alignment { get; }

        /// <summary>
        /// The value can be the default value or empty for strings.
        /// </summary>
        bool CanBeEmpty { get; }

        /// <summary>
        /// The value can be null.
        /// </summary>
        bool CanBeNull { get; }

        /// <summary>
        /// The field description
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// The field display name
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The value is editable.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Maximum value or maximum length for strings
        /// </summary>
        int Max { get; }

        /// <summary>
        /// Minimum value or minimum length for strings
        /// </summary>
        int Min { get; }

        /// <summary>
        /// The field name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The field display name
        /// </summary>
        string ShortDisplayName { get; }
    }
}
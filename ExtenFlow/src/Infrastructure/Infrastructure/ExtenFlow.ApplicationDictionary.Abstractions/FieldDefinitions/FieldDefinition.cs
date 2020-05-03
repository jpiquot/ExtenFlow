using System;

namespace ExtenFlow.ApplicationDictionary.FieldDefinitions
{
    /// <summary>
    /// String field/property definition
    /// </summary>
    public abstract class FieldDefinition<T> : IFieldDefinition
    {
        private readonly FieldAlignment? _alignment;
        private readonly bool? _canBeEmpty;
        private readonly bool? _canBeNull;
        private readonly string? _description;
        private readonly string? _displayName;
        private readonly Func<string, FieldDefinition<T>> _getFieldDefinition;
        private readonly int? _max;
        private readonly int? _min;
        private readonly string _name;
        private readonly string? _parentName;
        private readonly bool? _readOnly;
        private readonly string? _shortDisplayName;
        private FieldDefinition<T>? _parent;

        /// <summary>
        /// Constructor
        /// </summary>
        protected FieldDefinition(
            string name,
            string? displayName,
            string? shortDisplayName,
            string? description,
            string? parentName,
            int? min,
            int? max,
            bool? canBeNull,
            bool? canBeEmpty,
            bool? readOnly,
            FieldAlignment? alignment,
            Func<string, FieldDefinition<T>> getFieldDefinition)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            _name = name;
            if (string.IsNullOrWhiteSpace(parentName))
            {
                _parentName = null;
            }
            else
            {
                _parentName = parentName;
            }
            if (string.IsNullOrWhiteSpace(displayName))
            {
                _displayName = null;
            }
            else
            {
                _displayName = displayName;
            }
            if (string.IsNullOrWhiteSpace(shortDisplayName))
            {
                _shortDisplayName = null;
            }
            else
            {
                _shortDisplayName = shortDisplayName;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                _description = null;
            }
            else
            {
                _description = description;
            }
            _min = min;
            _max = max;
            _canBeNull = canBeNull;
            _canBeEmpty = canBeEmpty;
            _readOnly = readOnly;
            _alignment = alignment;
            _getFieldDefinition = getFieldDefinition;
        }

        /// <summary>
        /// The value is editable.
        /// </summary>
        public FieldAlignment Alignment => _alignment ?? _parent?.Alignment ?? FieldAlignment.Left;

        /// <summary>
        /// The value can be the default value or empty for strings.
        /// </summary>
        public bool CanBeEmpty => _canBeEmpty ?? _parent?.CanBeEmpty ?? true;

        /// <summary>
        /// The value can be null.
        /// </summary>
        public bool CanBeNull => _canBeNull ?? _parent?.CanBeNull ?? true;

        /// <summary>
        /// The field description
        /// </summary>
        public string? Description => _description;

        /// <summary>
        /// The field display name
        /// </summary>
        public string DisplayName => _displayName ?? Name;

        /// <summary>
        /// The value is editable.
        /// </summary>
        public bool IsReadOnly => _readOnly ?? _parent?.IsReadOnly ?? false;

        /// <summary>
        /// Maximum value or maximum length for strings
        /// </summary>
        public int Max => _max ?? _parent?.Max ?? 0;

        /// <summary>
        /// Minimum value or minimum length for strings
        /// </summary>
        public int Min => _min ?? _parent?.Min ?? 0;

        /// <summary>
        /// The field name
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// The field display name
        /// </summary>
        public string ShortDisplayName => _shortDisplayName ?? Name;

        /// <summary>
        /// The parent definition
        /// </summary>
        protected FieldDefinition<T>? Parent => (_parentName == null) ? null : _parent ?? (_parent = _getFieldDefinition(_parentName));
    }
}
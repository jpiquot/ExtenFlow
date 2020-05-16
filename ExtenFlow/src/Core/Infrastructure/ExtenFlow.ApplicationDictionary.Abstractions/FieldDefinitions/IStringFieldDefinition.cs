namespace ExtenFlow.ApplicationDictionary.FieldDefinitions
{
    /// <summary>
    /// String field/property definition interface
    /// </summary>
    public interface IStringFieldDefinition : IFieldDefinition
    {
        /// <summary>
        /// The string can be empty
        /// </summary>
        public bool CanBeWhiteSpaces { get; }
    }
}
using System;

namespace ExtenFlow.ApplicationDictionary.FieldDefinitions
{
    /// <summary>
    /// Identifier field definition
    /// </summary>
    public class IdentifierFieldDefinition : StringFieldDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected IdentifierFieldDefinition(
            Func<string, StringFieldDefinition> getFieldDefinition)
            : base(Properties.Resources.Identifier, Properties.Resources.Id, Properties.Resources.Id, null, null, 1, 22, false, false, false, false, FieldAlignment.Left, getFieldDefinition)
        {
        }
    }
}
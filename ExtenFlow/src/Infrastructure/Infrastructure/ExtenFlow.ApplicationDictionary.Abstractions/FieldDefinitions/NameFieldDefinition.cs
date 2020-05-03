using System;

namespace ExtenFlow.ApplicationDictionary.FieldDefinitions
{
    /// <summary>
    /// Name field definition
    /// </summary>
    public class NameFieldDefinition : StringFieldDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected NameFieldDefinition(
            Func<string, StringFieldDefinition> getFieldDefinition)
            : base(Properties.Resources.Name, null, null, null, null, 1, 254, false, false, false, false, FieldAlignment.Left, getFieldDefinition)
        {
        }
    }
}
using System;

namespace ExtenFlow.ApplicationDictionary.FieldDefinitions
{
    /// <summary>
    /// Name field definition
    /// </summary>
    public class DescriptionFieldDefinition : StringFieldDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected DescriptionFieldDefinition(
            Func<string, StringFieldDefinition> getFieldDefinition)
            : base(Properties.Resources.Description, null, null, null, null, 0, 0, false, false, false, false, FieldAlignment.Left, getFieldDefinition)
        {
        }
    }
}
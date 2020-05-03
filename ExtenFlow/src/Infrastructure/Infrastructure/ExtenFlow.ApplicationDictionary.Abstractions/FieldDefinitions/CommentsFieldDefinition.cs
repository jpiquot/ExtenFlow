using System;

namespace ExtenFlow.ApplicationDictionary.FieldDefinitions
{
    /// <summary>
    /// Name field definition
    /// </summary>
    public class CommentsFieldDefinition : StringFieldDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected CommentsFieldDefinition(
            Func<string, StringFieldDefinition> getFieldDefinition)
            : base(Properties.Resources.Comments, null, null, null, null, 0, 0, false, false, false, false, FieldAlignment.Left, getFieldDefinition)
        {
        }
    }
}
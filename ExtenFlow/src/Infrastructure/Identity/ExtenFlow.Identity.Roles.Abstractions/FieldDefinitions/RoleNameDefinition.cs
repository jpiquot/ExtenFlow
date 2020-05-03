using System;

using ExtenFlow.ApplicationDictionary.FieldDefinitions;
using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.Roles.FieldDefinitions
{
    /// <summary>
    /// Role name definition
    /// </summary>
    public class RoleNameDefinition : StringFieldDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="getFieldDefinition"></param>
        public RoleNameDefinition(Func<string, StringFieldDefinition> getFieldDefinition)
            : base(
                  name: $"{nameof(Role)}.{nameof(Role.Name)}",
                  displayName: null,
                  shortDisplayName: null,
                  description: null,
                  parentName: typeof(NameFieldDefinition).AssemblyQualifiedName,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  null,
                  getFieldDefinition
                  )
        {
        }
    }
}
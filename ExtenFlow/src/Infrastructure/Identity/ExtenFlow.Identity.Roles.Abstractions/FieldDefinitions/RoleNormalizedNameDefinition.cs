using System;

using ExtenFlow.ApplicationDictionary.FieldDefinitions;
using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.Roles.FieldDefinitions
{
    /// <summary>
    /// Role name definition
    /// </summary>
    public class RoleNormalizedNameDefinition : StringFieldDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="getFieldDefinition"></param>
        public RoleNormalizedNameDefinition(Func<string, StringFieldDefinition> getFieldDefinition)
            : base(
                  name: $"{nameof(Role)}.{nameof(Role.NormalizedName)}",
                  displayName: Properties.Resources.NormalizedName,
                  shortDisplayName: Properties.Resources.Normalized,
                  description: Properties.Resources.NormalizedNameDescription,
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
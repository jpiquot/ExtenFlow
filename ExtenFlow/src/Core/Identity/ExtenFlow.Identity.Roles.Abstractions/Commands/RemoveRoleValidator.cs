using ExtenFlow.Identity.Roles.Commands;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Remove role command validation
    /// </summary>
    public class RemoveRoleValidator : RoleCommandValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RemoveRoleValidator() : base(nameof(RemoveRole))
        {
        }
    }
}
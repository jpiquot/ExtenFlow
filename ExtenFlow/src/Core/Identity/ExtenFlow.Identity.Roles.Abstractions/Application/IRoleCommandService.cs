using ExtenFlow.Domain.Commands;
using ExtenFlow.Identity.Roles.Commands;

namespace ExtenFlow.Identity.Roles.Application
{
    /// <summary>
    /// Interface IRoleService
    /// </summary>
    public interface IRoleCommandService :
        ICommandHandler<AddNewRole>,
        ICommandHandler<AddRoleClaim>,
        ICommandHandler<AddUserToRole>,
        ICommandHandler<RegisterNormalizedRoleName>,
        ICommandHandler<DeregisterNormalizedRoleName>,
        ICommandHandler<RemoveRole>,
        ICommandHandler<RemoveRoleClaim>,
        ICommandHandler<RemoveUserFromRole>,
        ICommandHandler<RenameRole>
    {
    }
}
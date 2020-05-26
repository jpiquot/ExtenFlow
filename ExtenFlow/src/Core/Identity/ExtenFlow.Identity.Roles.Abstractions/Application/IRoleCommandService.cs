using ExtenFlow.Domain.Commands;
using ExtenFlow.Identity.Roles.Commands;

namespace ExtenFlow.Identity.Roles.Application
{
    /// <summary>
    /// Interface IRoleService
    /// </summary>
    public interface IRoleCommandService :
        ICommandHandler<AddNewRole>,
        ICommandHandler<RemoveRole>,
        ICommandHandler<RenameRole>,
        ICommandHandler<AddRoleClaim>,
        ICommandHandler<RemoveRoleClaim>
    {
    }
}
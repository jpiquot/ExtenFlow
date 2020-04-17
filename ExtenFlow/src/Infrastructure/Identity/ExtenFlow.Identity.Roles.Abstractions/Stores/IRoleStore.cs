using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Roles.Stores
{
    /// <summary>
    /// Role store interface
    /// </summary>
    /// <seealso cref="Role"/>
    public interface IRoleStore : IRoleStore<Role>, IQueryableRoleStore<Role>, IRoleClaimStore<Role>
    {
    }
}
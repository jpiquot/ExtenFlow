using ExtenFlow.IdentityServer.Domain.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.IdentityServer.Stores
{
    /// <summary>
    /// Role store interface
    /// </summary>
    /// <seealso cref="Role"/>
    public interface IRoleStore : IRoleStore<Role>, IQueryableRoleStore<Role>, IRoleClaimStore<Role>
    {
    }
}
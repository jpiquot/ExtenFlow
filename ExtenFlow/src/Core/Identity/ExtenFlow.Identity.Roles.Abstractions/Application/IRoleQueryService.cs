using System.Collections.Generic;
using System.Security.Claims;

using ExtenFlow.Domain.Commands;
using ExtenFlow.Identity.Roles.Queries;

namespace ExtenFlow.Identity.Roles.Application
{
    /// <summary>
    /// Interface IRoleService
    /// </summary>
    public interface IRoleQueryService :
        IQueryHandler<GetRoleClaims, IList<Claim>>,
        IQueryHandler<GetRoleDetails, RoleDetails>,
        IQueryHandler<GetRoleIdByName, string>,
        IQueryHandler<FindRoleIdByName, string?>,
        IQueryHandler<IsRoleNameRegistered, bool>
    {
    }
}
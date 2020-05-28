using System.Collections.Generic;
using System.Security.Claims;

using ExtenFlow.Domain.Commands;
using ExtenFlow.Identity.Roles.Queries;

namespace ExtenFlow.Identity.Roles.Application
{
    /// <summary>
    /// The role consistent queries interface
    /// </summary>
    /// <remarks>
    /// These queries are applied to the aggregate write model. Don't use these queries if data
    /// constitency is not mandatory. Use the <see cref="IRoleQueryService"/> for fast eventually
    /// consistent queries.
    /// </remarks>
    public interface IRoleConsistentQueryService :
        IQueryHandler<GetRoleClaims, IList<Claim>>,
        IQueryHandler<GetRoleDetails, RoleDetails>,
        IQueryHandler<GetRoleIdByName, string>,
        IQueryHandler<FindRoleIdByName, string?>,
        IQueryHandler<IsRoleNameRegistered, bool>
    {
    }
}
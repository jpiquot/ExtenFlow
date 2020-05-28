using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using ExtenFlow.Domain.Commands;
using ExtenFlow.Identity.Roles.Models;
using ExtenFlow.Identity.Roles.Queries;

namespace ExtenFlow.Identity.Roles.Application
{
    /// <summary>
    /// The role eventually consistent queries interface
    /// </summary>
    /// <remarks>
    /// These queries are applied to read only data models. Don't use these queries if data
    /// constitency is mandatory. Use the <see cref="IRoleConsistentQueryService"/> for fast
    /// eventually consistent queries.
    /// </remarks>
    public interface IRoleQueryService :
        IQueryHandler<GetRoleClaims, IList<Claim>>,
        IQueryHandler<GetRoleDetails, RoleDetails>,
        IQueryHandler<GetRoleIdByName, string>,
        IQueryHandler<FindRoleIdByName, string?>,
        IQueryHandler<IsRoleNameRegistered, bool>
    {
        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        IQueryable<Role> Roles { get; }
    }
}
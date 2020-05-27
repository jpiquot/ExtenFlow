﻿using System.Collections.Generic;
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
        IQueryHandler<GetRoleDetails, RoleDetailsModel>,
        IQueryHandler<GetRoleIdByName, string?>,
        IQueryHandler<IsRoleNameRegistered, bool>
    {
    }
}
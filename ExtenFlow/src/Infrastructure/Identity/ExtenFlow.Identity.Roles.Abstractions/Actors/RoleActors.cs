﻿using System;
using System.Resources;

using Dapr.Actors;
using Dapr.Actors.Client;

#pragma warning disable IDE0060 // Remove unused parameter
[assembly: NeutralResourcesLanguage("en")]

namespace ExtenFlow.Identity.Roles.Actors
{
    /// <summary>
    /// Role actors helper
    /// </summary>
    public static class RoleActors
    {
        /// <summary>
        /// Creates the role actor.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The actor instance interface</returns>
        public static IRoleActor Role(string roleId) => ActorProxy.Create<IRoleActor>(new ActorId(roleId), "RoleActor");

        /// <summary>
        /// Roles the claims.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>IRoleClaimsActor.</returns>
        public static IRoleClaimsActor RoleClaims(string roleId) => ActorProxy.Create<IRoleClaimsActor>(new ActorId(roleId), "RoleClaimsActor");
    }
}
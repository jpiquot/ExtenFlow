using System;
using System.Resources;
using Dapr.Actors;
using Dapr.Actors.Client;

#pragma warning disable IDE0060 // Remove unused parameter
[assembly: NeutralResourcesLanguage("en")]

namespace ExtenFlow.Identity.Roles
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
        public static IRoleActor Role(Guid roleId) => ActorProxy.Create<IRoleActor>(new ActorId(roleId.ToString()), "RoleActor");
    }
}
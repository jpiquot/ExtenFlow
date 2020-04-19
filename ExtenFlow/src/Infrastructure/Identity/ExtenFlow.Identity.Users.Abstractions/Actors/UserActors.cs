using System;
using System.Resources;

using Dapr.Actors;
using Dapr.Actors.Client;

#pragma warning disable IDE0060 // Remove unused parameter
[assembly: NeutralResourcesLanguage("en")]

namespace ExtenFlow.Identity.Users.Actors
{
    /// <summary>
    /// User actors helper
    /// </summary>
    public static class UserActors
    {
        /// <summary>
        /// Creates the user actor.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The actor instance interface</returns>
        public static IUserActor User(Guid userId) => ActorProxy.Create<IUserActor>(new ActorId(userId.ToString()), "UserActor");

        /// <summary>
        /// Creates the user claims actor.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The actor instance interface</returns>
        public static IUserClaimsActor UserClaims(Guid userId) => ActorProxy.Create<IUserClaimsActor>(new ActorId(userId.ToString()), "UserClaimsActor");
    }
}
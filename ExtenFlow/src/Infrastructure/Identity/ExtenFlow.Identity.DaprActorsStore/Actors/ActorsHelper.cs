using System;

using Dapr.Actors;
using Dapr.Actors.Client;

#pragma warning disable IDE0060 // Remove unused parameter

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// Identity Actors Proxy helper
    /// </summary>
    public static class IdentityActors
    {
        /// <summary>
        /// Creates the role actor.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The actor instance interface</returns>
        public static IRoleActor Role(Guid roleId) => ActorProxy.Create<IRoleActor>(new ActorId(roleId.ToString()), nameof(RoleActor));

        /// <summary>
        /// Creates the role claims actor.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The actor instance interface</returns>
        public static IRoleClaimsActor RoleClaims(Guid roleId) => ActorProxy.Create<IRoleClaimsActor>(new ActorId(roleId.ToString()), nameof(RoleClaimsActor));

        /// <summary>
        /// Creates a role claims collection actor.
        /// </summary>
        /// <returns>The actor instance interface</returns>
        public static IRoleClaimsCollectionActor RoleClaimsCollection() => ActorProxy.Create<IRoleClaimsCollectionActor>(new ActorId(string.Empty), nameof(RoleClaimsCollectionActor));

        /// <summary>
        /// Creates a roles collection actor.
        /// </summary>
        /// <returns>The actor instance interface</returns>
        public static IRoleCollectionActor RoleCollection() => ActorProxy.Create<IRoleCollectionActor>(new ActorId(string.Empty), nameof(RoleCollectionActor));

        /// <summary>
        /// Creates the user actor.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The actor instance interface</returns>
        public static IUserActor User(Guid userId) => ActorProxy.Create<IUserActor>(new ActorId(userId.ToString()), nameof(UserActor));

        /// <summary>
        /// Creates the user claims actor.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The actor instance interface</returns>
        public static IUserClaimsActor UserClaims(Guid userId) => ActorProxy.Create<IUserClaimsActor>(new ActorId(userId.ToString()), nameof(UserClaimsActor));

        /// <summary>
        /// Creates a user claims collection actor.
        /// </summary>
        /// <returns>The actor instance interface</returns>
        public static IUserClaimsCollectionActor UserClaimsCollection() => ActorProxy.Create<IUserClaimsCollectionActor>(new ActorId(string.Empty), nameof(UserClaimsCollectionActor));

        /// <summary>
        /// Creates a users collection actor.
        /// </summary>
        /// <returns>The actor instance interface</returns>
        public static IUserCollectionActor UserCollection() => ActorProxy.Create<IUserCollectionActor>(new ActorId(string.Empty), nameof(UserCollectionActor));

        /// <summary>
        /// Creates the user login actor.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The actor instance interface</returns>
        public static IUserLoginsActor UserLogin(Guid userId) => ActorProxy.Create<IUserLoginsActor>(new ActorId(userId.ToString()), nameof(UserLoginsActor));

        /// <summary>
        /// Creates a user logins collection actor.
        /// </summary>
        /// <returns>The actor instance interface</returns>
        public static IUserLoginsCollectionActor UserLoginsCollection() => ActorProxy.Create<IUserLoginsCollectionActor>(new ActorId(string.Empty), nameof(UserLoginsCollectionActor));

        /// <summary>
        /// Creates a user role collection actor.
        /// </summary>
        /// <returns>The actor instance interface</returns>
        public static IUserRoleCollectionActor UserRoleCollection() => ActorProxy.Create<IUserRoleCollectionActor>(new ActorId(string.Empty), nameof(UserRoleCollectionActor));

        /// <summary>
        /// Creates the user token actor.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The actor instance interface</returns>
        public static IUserTokensActor UserTokens(Guid userId) => ActorProxy.Create<IUserTokensActor>(new ActorId(userId.ToString()), nameof(UserTokensActor));
    }
}
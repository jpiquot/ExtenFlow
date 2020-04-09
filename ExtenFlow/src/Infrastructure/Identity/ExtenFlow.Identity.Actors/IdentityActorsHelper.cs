using System;

using Dapr.Actors.Runtime;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// Helper methods for identity actors.
    /// </summary>
    public static class IdentityActorsHelper
    {
        /// <summary>
        /// Registers the identity actors.
        /// </summary>
        /// <param name="actorRuntime">The actor runtime.</param>
        public static void RegisterIdentityActors(this ActorRuntime actorRuntime)
        {
            if (actorRuntime == null)
            {
                throw new ArgumentNullException(nameof(actorRuntime));
            }
            actorRuntime.RegisterActor<RoleActor>(information
                => new ActorService(information, (service, id)
                    => new RoleActor(service, id, CollectionServiceProxyFactory.Create<IdentityCollectionActor, RoleActor>())));

            actorRuntime.RegisterActor<RoleClaimsActor>(information => new ActorService(information, (service, id) => new RoleClaimsActor(service, id)));
            actorRuntime.RegisterActor<RoleClaimsCollectionActor>(information => new ActorService(information, (service, id) => new RoleClaimsCollectionActor(service, id)));
            actorRuntime.RegisterActor<UserActor>(information => new ActorService(information, (service, id) => new UserActor(service, id)));
            actorRuntime.RegisterActor<UserCollectionActor>(information => new ActorService(information, (service, id) => new UserCollectionActor(service, id)));
            actorRuntime.RegisterActor<UserClaimsActor>(information => new ActorService(information, (service, id) => new UserClaimsActor(service, id)));
            actorRuntime.RegisterActor<UserClaimsCollectionActor>(information => new ActorService(information, (service, id) => new UserClaimsCollectionActor(service, id)));
            actorRuntime.RegisterActor<UserLoginsActor>(information => new ActorService(information, (service, id) => new UserLoginsActor(service, id)));
            actorRuntime.RegisterActor<UserLoginsCollectionActor>(information => new ActorService(information, (service, id) => new UserLoginsCollectionActor(service, id)));
            actorRuntime.RegisterActor<UserRoleCollectionActor>(information => new ActorService(information, (service, id) => new UserRoleCollectionActor(service, id)));
            actorRuntime.RegisterActor<UserTokensActor>(information => new ActorService(information, (service, id) => new UserTokensActor(service, id)));
        }
    }
}
using System;

using Dapr.Actors.Runtime;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Roles
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
        }
    }
}
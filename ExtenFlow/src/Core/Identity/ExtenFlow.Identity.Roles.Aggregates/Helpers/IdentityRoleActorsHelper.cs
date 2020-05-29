using System;

using Dapr.Actors.Runtime;

using ExtenFlow.EventBus;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Roles.Application;

namespace ExtenFlow.Identity.Roles.Helpers
{
    /// <summary>
    /// Helper methods for identity actors.
    /// </summary>
    public static class IdentityRoleActorsHelper
    {
        /// <summary>
        /// Registers the identity actors.
        /// </summary>
        /// <param name="actorRuntime">The actor runtime.</param>
        /// <param name="eventBus"></param>
        /// <param name="eventStore"></param>
        public static void RegisterRoleActors(this ActorRuntime actorRuntime, IEventBus eventBus, IEventStore eventStore)
        {
            if (actorRuntime == null)
            {
                throw new ArgumentNullException(nameof(actorRuntime));
            }
            actorRuntime.RegisterActor<RoleActor>(information
                => new ActorService(information, (service, id)
                    => new RoleActor(
                        service,
                        id,
                        eventBus,
                        eventStore
                        )
                ));
            actorRuntime.RegisterActor<RoleNameActor>(information
                => new ActorService(information, (service, id)
                    => new RoleNameActor(
                        service,
                        id,
                        eventBus,
                        eventStore
                        )
                ));
        }
    }
}
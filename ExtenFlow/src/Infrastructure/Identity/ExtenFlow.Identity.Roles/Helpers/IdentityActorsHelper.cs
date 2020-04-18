using System;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Roles.Actors;
using ExtenFlow.Messages.Dispatcher;

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
        public static void RegisterIdentityRoleActors(this ActorRuntime actorRuntime, IEventBus eventBus, IEventStore eventStore)
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
                        ActorProxy.Create<IUniqueIndexActor>(new ActorId(ActorHelper.ActorName<RoleActor>()), nameof(UniqueIndexActor)),
                        eventBus,
                        eventStore
                        )
                ));
            actorRuntime.RegisterActor<RoleClaimsActor>(information
                => new ActorService(information, (service, id)
                    => new RoleClaimsActor(
                        service,
                        id,
                        eventBus,
                        eventStore
                        )
                ));
        }
    }
}
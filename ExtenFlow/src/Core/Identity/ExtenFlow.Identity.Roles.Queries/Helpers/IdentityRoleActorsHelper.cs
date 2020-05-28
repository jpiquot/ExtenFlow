using System;

using Dapr.Actors.Runtime;

using ExtenFlow.Domain.Dispatcher;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Roles.Queries;

namespace ExtenFlow.Identity.Roles.Queries
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
            actorRuntime.RegisterActor<RoleRelationalModelActor>(information
                => new ActorService(information, (service, id)
                    => new RoleRelationalModelActor(
                        service,
                        id,
                        eventBus,
                        eventStore
                        )
                ));
        }
    }
}
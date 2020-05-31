using System;

using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Identity.Roles.Application;
using ExtenFlow.Messages.Events;

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
        /// <param name="eventPublisher">
        /// The event publisher used to send events on the domain integration bus.
        /// </param>
        /// <param name="eventStore">The event store</param>
        public static void RegisterRoleActors(this ActorRuntime actorRuntime, IEventPublisherBuilder eventPublisher, IEventStoreBuilder eventStore)
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
                        eventPublisher.Build(),
                        eventStore.Name<RoleActor>(id).Build()
                        )
                ));
            actorRuntime.RegisterActor<RoleNameRegistryActor>(information
                => new ActorService(information, (service, id)
                    => new RoleNameRegistryActor(
                        service,
                        id,
                        eventPublisher.Build(),
                        eventStore.Name<RoleNameRegistryActor>(id).Build()
                        )
                ));
        }
    }
}
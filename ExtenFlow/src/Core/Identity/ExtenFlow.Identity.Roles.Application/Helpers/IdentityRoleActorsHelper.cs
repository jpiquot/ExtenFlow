using System;

using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Roles.Application;
using ExtenFlow.Messages.Events;

namespace ExtenFlow.Identity.Roles.Application.Helpers
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
        public static void RegisterRoleActors(this ActorRuntime actorRuntime, Func<IEventPublisher> eventPublisher, Func<string, IEventStore> eventStore)
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
                        eventPublisher(),
                        eventStore(id.GetId())
                        )
                ));
            actorRuntime.RegisterActor<RoleNameRegistryActor>(information
                => new ActorService(information, (service, id)
                    => new RoleNameRegistryActor(
                        service,
                        id,
                        eventPublisher(),
                        eventStore(id.GetId())
                        )
                ));
        }
    }
}
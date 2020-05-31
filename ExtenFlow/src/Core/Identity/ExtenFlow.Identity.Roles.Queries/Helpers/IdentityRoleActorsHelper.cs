using System;

using Dapr.Actors.Runtime;

using ExtenFlow.EventStorage;
using ExtenFlow.Messages.Events;

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
        /// <param name="eventPublisher">
        /// The event publisher used to send events on the domain integration bus.
        /// </param>
        /// <param name="eventStore">The event store</param>
        public static void RegisterIdentityRoleActors(this ActorRuntime actorRuntime, IEventPublisher eventPublisher, IEventStore eventStore)
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
                        eventPublisher,
                        eventStore
                        )
                ));
        }
    }
}
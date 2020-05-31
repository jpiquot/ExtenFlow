using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Roles.Domain;
using ExtenFlow.Messages.Events;

namespace ExtenFlow.Identity.Roles.Application
{
    /// <summary>
    /// Class RoleActor. Implements the <see cref="ExtenFlow.Actors.AggregateRootActor"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Actors.AggregateRootActor"/>
    public class RoleNameActor : AggregateRootActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="eventPublisher">
        /// The event publisher used to send events on the domain integration bus.
        /// </param>
        /// <param name="eventStore">The event store</param>
        /// <param name="actorStateManager">The actor state manager.</param>
        public RoleNameActor(
             ActorService actorService,
             ActorId actorId,
             IEventPublisher eventPublisher,
             IEventStore eventStore,
             IActorStateManager? actorStateManager = null)
            : base(
                  actorService,
                  actorId,
                  (id, stateManager) => new RoleNameRegistryAggregateRoot(id, new ActorRepository(stateManager)),
                  eventPublisher,
                  eventStore,
                  actorStateManager)
        {
        }
    }
}
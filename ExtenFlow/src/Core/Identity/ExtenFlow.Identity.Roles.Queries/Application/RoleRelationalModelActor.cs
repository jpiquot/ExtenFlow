using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Domain.Dispatcher;
using ExtenFlow.EventStorage;

namespace ExtenFlow.Identity.Roles.Queries
{
    /// <summary>
    /// Class RoleCollectionActor. Implements the <see cref="ExtenFlow.Actors.QueryActor"/>
    /// Implements the <see cref="IRoleRelationalModelActor"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Actors.QueryActor"/>
    /// <seealso cref="IRoleRelationalModelActor"/>
    public class RoleRelationalModelActor : QueryActor, IRoleRelationalModelActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRelationalModelActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="eventStore">The event store.</param>
        /// <param name="actorStateManager">The actor state manager.</param>
        public RoleRelationalModelActor(
            ActorService actorService,
            ActorId actorId,
            IEventBus eventBus,
            IEventStore eventStore,
            IActorStateManager? actorStateManager = null)
            : base(actorService, actorId, eventBus, eventStore, actorStateManager)

        {
        }
    }
}
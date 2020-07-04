using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.IdentityServer.Application.Actors;
using ExtenFlow.IdentityServer.Domain;
using ExtenFlow.Messages.Events;

namespace ExtenFlow.IdentityServer.Application
{
    /// <summary>
    /// Class Client Actor. Implements the <see cref="ExtenFlow.Actors.AggregateRootActor"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Actors.AggregateRootActor"/>
    public class ClientActor : AggregateRootActor, IClientActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="eventPublisher">
        /// The event publisher used to send events on the domain integration bus.
        /// </param>
        /// <param name="eventStore">The event store</param>
        /// <param name="actorStateManager">The actor state manager.</param>
        public ClientActor(
             ActorService actorService,
             ActorId actorId,
             IEventPublisher eventPublisher,
             IEventStore eventStore,
             IActorStateManager? actorStateManager = null)
            : base(
                  actorService,
                  actorId,
                  (id, stateManager) => new ClientAggregateRoot(id, new ActorRepository(stateManager)),
                  eventPublisher,
                  eventStore,
                  actorStateManager)
        {
        }
    }
}
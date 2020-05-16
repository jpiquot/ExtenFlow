using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.EventStorage;
using ExtenFlow.Domain.Dispatcher;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Class EventSourceActorBase. Implements the <see cref="ExtenFlow.Actors.DispatchActorBase{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ExtenFlow.Actors.DispatchActorBase{T}"/>
    public abstract class EventSourcedActorBase<T> : DispatchActorBase<T>
    {
        private readonly IEventStore _eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSourcedActorBase{T}"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="messageQueue">The message queue used to publish events.</param>
        /// <param name="eventStore">The event store used to persist events.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        protected EventSourcedActorBase
        (
            ActorService actorService,
            ActorId actorId,
            IEventBus messageQueue,
            IEventStore eventStore,
            IActorStateManager? actorStateManager)
            : base(actorService, actorId, messageQueue, actorStateManager)
        {
            _eventStore = eventStore;
        }
    }
}
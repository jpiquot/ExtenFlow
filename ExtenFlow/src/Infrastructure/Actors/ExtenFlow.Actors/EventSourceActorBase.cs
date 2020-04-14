using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.EventStorage;
using ExtenFlow.Messages.Dispatcher;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Class EventSourceActorBase. Implements the <see cref="ExtenFlow.Actors.DispatchActorBase{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ExtenFlow.Actors.DispatchActorBase{T}"/>
    public abstract class EventSourceActorBase<T> : DispatchActorBase<T>
        where T : class, new()
    {
        private readonly IEventStore _eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSourceActorBase{T}"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="messageQueue">The message queue.</param>
        /// <param name="eventStore"></param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        protected EventSourceActorBase
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
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Messages;
using ExtenFlow.Messages.Dispatcher;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Dispatch actor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ExtenFlow.Actors.ActorBase{T}"/>
    public abstract class DispatchActorBase<T> : ActorBase<T>, IMessageDispatcher
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchActorBase{T}"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public DispatchActorBase(ActorService actorService, ActorId actorId, IActorStateManager actorStateManager) : base(actorService, actorId, actorStateManager)
        {
        }

        public Task<object> Ask(Envelope envelope)
        {
        }

        public Task Send(Envelope envelope)
        {
        }

        public Task Submit(Envelope envelope)
        {
        }
    }
}
using System;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.EventBus;
using ExtenFlow.EventStorage;
using ExtenFlow.Messages;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Class QueryActor. Implements the <see cref="ExtenFlow.Actors.DispatchActorBase"/> Implements
    /// the <see cref="ExtenFlow.Actors.IQueryActor"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Actors.DispatchActorBase"/>
    /// <seealso cref="ExtenFlow.Actors.IQueryActor"/>
    public class QueryActor : DispatchActorBase, IQueryActor
    {
        private readonly IEventStore _eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="eventStore"></param>
        /// <param name="actorStateManager">The actor state manager.</param>
        protected QueryActor(
            ActorService actorService,
            ActorId actorId,
            IEventBus eventBus,
            IEventStore eventStore,
            IActorStateManager? actorStateManager)
            : base(actorService, actorId, eventBus, actorStateManager)
        {
            _eventStore = eventStore;
        }

        /// <summary>
        /// Receives the event.
        /// </summary>
        /// <param name="event">The event message.</param>
        /// <param name="batchSave"></param>
        /// <returns>Task.</returns>
        protected override Task ReceiveEvent(IEvent @event, bool batchSave = false)
            => Task.CompletedTask;

        /// <summary>
        /// Receive a notification message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>List of generated events.</returns>
        protected override Task ReceiveNotification(IMessage message) => Task.CompletedTask;

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        protected override Task<object> ReceiveQuery(IQuery query)
            => throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.QueryNotSupported, query?.GetType().Name, GetType().Name));
    }
}
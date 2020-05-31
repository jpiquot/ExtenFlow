using System;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Messages;
using ExtenFlow.Messages.Events;

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
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="eventPublisher">
        /// The event publisher used to send events on the domain integration bus.
        /// </param>
        /// <param name="eventStore">The event store reader.</param>
        /// <param name="actorStateManager">The actor state manager.</param>
        protected QueryActor(
            ActorService actorService,
            ActorId actorId,
            IEventPublisher eventPublisher,
            IEventStoreReader eventStore,
            IActorStateManager? actorStateManager)
            : base(actorService, actorId, eventPublisher, actorStateManager)
        {
            EventStore = eventStore;
        }

        /// <summary>
        /// Gets the event store reader.
        /// </summary>
        /// <value>The event store.</value>
        protected IEventStoreReader EventStore { get; }

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
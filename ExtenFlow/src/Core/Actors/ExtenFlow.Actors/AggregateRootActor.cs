using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Domain;
using ExtenFlow.Domain.Aggregates;
using ExtenFlow.Domain.Dispatcher;
using ExtenFlow.EventStorage;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Class AggregateRootActor. Implements the <see cref="ExtenFlow.Actors.DispatchActorBase"/>
    /// Implements the <see cref="ExtenFlow.Actors.IAggregateRootActor"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Actors.DispatchActorBase"/>
    /// <seealso cref="ExtenFlow.Actors.IAggregateRootActor"/>
    public class AggregateRootActor : DispatchActorBase, IAggregateRootActor
    {
        private readonly IEventStore _eventStore;
        private readonly Func<string, IActorStateManager, IAggregateRoot> _getAggregateRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRootActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="getAggregateRoot">The get aggregate root.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="eventStore"></param>
        /// <param name="actorStateManager">The actor state manager.</param>
        protected AggregateRootActor(
            ActorService actorService,
            ActorId actorId,
            Func<string, IActorStateManager, IAggregateRoot> getAggregateRoot,
            IEventBus eventBus,
            IEventStore eventStore,
            IActorStateManager? actorStateManager)
            : base(actorService, actorId, eventBus, actorStateManager)
        {
            _getAggregateRoot = getAggregateRoot;
            _eventStore = eventStore;
        }

        /// <summary>
        /// Gets the aggregate root.
        /// </summary>
        /// <value>The aggregate root.</value>
        protected IAggregateRoot AggregateRoot => _getAggregateRoot(Id.GetId(), StateManager);

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;IList&lt;IEvent&gt;&gt;.</returns>
        protected override Task<IList<IEvent>> ReceiveCommand(ICommand command)
            => AggregateRoot.HandleCommand(command);

        /// <summary>
        /// Receives the event.
        /// </summary>
        /// <param name="event">The event message.</param>
        /// <param name="batchSave"></param>
        /// <returns>Task.</returns>
        protected override Task ReceiveEvent(IEvent @event, bool batchSave = false)
            => AggregateRoot.HandleEvent(@event);

        /// <summary>
        /// Receive a notification message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>List of generated events.</returns>
        protected override Task ReceiveNotification(IMessage message) => AggregateRoot.HandleNotification(message);

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        protected override Task<object> ReceiveQuery(IQuery query) => AggregateRoot.HandleQuery(query);
    }
}
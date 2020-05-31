using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Messages;
using ExtenFlow.Messages.Events;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Dispatch actor
    /// </summary>
    /// <seealso cref="ExtenFlow.Actors.DispatchActorBase"/>
    public abstract class DispatchActorBase : Actor, IDispatchActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchActorBase"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="eventPublisher">
        /// The event publisher used to send events on the domain integration bus.
        /// </param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        protected DispatchActorBase(
            ActorService actorService,
            ActorId actorId,
            IEventPublisher eventPublisher,
            IActorStateManager? actorStateManager)
            : base(actorService, actorId, actorStateManager)
        {
            EventPublisher = eventPublisher;
        }

        /// <summary>
        /// Gets the event publisher.
        /// </summary>
        /// <value>The message queue.</value>
        protected IEventPublisher EventPublisher { get; }

        /// <summary>
        /// Asks to execute a query.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The query result</returns>
        public async Task<object> Ask(Envelope envelope)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }
            if (!(envelope.Message is IQuery query))
            {
                throw new ArgumentOutOfRangeException(nameof(envelope), Properties.Resources.MessageNotQuery);
            }
            if (Id.GetId() != query.AggregateId)
            {
                // Message aggregate identifier mismatch. Expected='{0}; Message='{1}'.
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.MessageAggregateIdMismatch, Id.GetId(), query.AggregateId));
            }
            return await ReceiveQuery(query);
        }

        /// <summary>
        /// Notify with the message in the envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        public async Task Notify(Envelope envelope)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }
            if (!(envelope.Message is IMessage message))
            {
                throw new ArgumentOutOfRangeException(nameof(envelope), Properties.Resources.ObjectNotMessage);
            }
            if (Id.GetId() != message.AggregateId)
            {
                // Message aggregate identifier mismatch. Expected='{0}; Message='{1}'.
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.MessageAggregateIdMismatch, Id.GetId(), message.AggregateId));
            }
            await ReceiveQueueMessage(message);
        }

        /// <summary>
        /// Tells to execute the command in the envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        public async Task Tell(Envelope envelope)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }
            if (!(envelope.Message is ICommand command))
            {
                throw new ArgumentOutOfRangeException(nameof(envelope), Properties.Resources.MessageNotCommand);
            }
            if (Id.GetId() != command.AggregateId)
            {
                // Message aggregate identifier mismatch. Expected='{0}; Message='{1}'.
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.MessageAggregateIdMismatch, Id.GetId(), command.AggregateId));
            }
            await HandleCommand(command);
        }

        /// <summary>
        /// This method is called whenever an actor is activated. An actor is activated the first
        /// time any of its methods are invoked.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            await ReceiveNotification(new ActorActivation(this));
        }

        /// <summary>
        /// This method is called whenever an actor is deactivated after a period of inactivity.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task OnDeactivateAsync()
        {
            await base.OnDeactivateAsync();
            await ReceiveNotification(new ActorDesactivation(this));
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        protected virtual Task<IList<IEvent>> ReceiveCommand(ICommand command)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            // The command '{0}' is not supported by '{1}'.
            throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.CommandNotSupported, command.GetType().Name, GetType().Name));
        }

        /// <summary>
        /// Receives the specified event.
        /// </summary>
        /// <param name="eventMessage">The event</param>
        /// <param name="batchSave">
        /// if set to <c>true</c> do not save data. It will be done at the end of the batch.
        /// </param>
        protected abstract Task ReceiveEvent(IEvent eventMessage, bool batchSave = false);

        /// <summary>
        /// Receive a notification message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>List of generated events.</returns>
        protected abstract Task ReceiveNotification(IMessage message);

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        protected abstract Task<object> ReceiveQuery(IQuery query);

        private async Task HandleCommand(ICommand command)
        {
            var events = await ReceiveCommand(command);
            foreach (IEvent anEvent in events)
            {
                await ReceiveEvent(anEvent, true);
            }
            await EventPublisher.Publish(events);
        }

        private Task ReceiveQueueMessage(IMessage message)
            => message switch
            {
                IQuery query => ReceiveQuery(query),
                ICommand command => HandleCommand(command),
                IEvent @event => ReceiveEvent(@event, false),
                _ => ReceiveNotification(message)
            };
    }
}
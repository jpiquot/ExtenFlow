using System;
using System.Collections.Generic;
using System.Globalization;
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
    public abstract class DispatchActorBase<T> : ActorBase<T>, IDispatchActor, IRemindable
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchActorBase{T}"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="messageQueue">The message queue.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        protected DispatchActorBase(
            ActorService actorService,
            ActorId actorId,
            IMessageQueue messageQueue,
            IActorStateManager? actorStateManager)
            : base(actorService, actorId, actorStateManager)
        {
            MessageQueue = messageQueue;
        }

        /// <summary>
        /// Gets the message queue.
        /// </summary>
        /// <value>The message queue.</value>
        public IMessageQueue MessageQueue { get; }

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
            await ReceiveAndProcessQueueMessages();
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
            await ReceiveAndProcessQueueMessages();
            await ReceiveQueueMessage(message);
        }

        /// <summary>
        /// Receives the and process messages.
        /// </summary>
        public async Task ReceiveAndProcessQueueMessages()
        {
            IMessage? message;
            while ((message = await MessageQueue.ReadNext()) != null)
            {
                await ReceiveQueueMessage(message);
                await MessageQueue.RemoveMessage(message.MessageId);
            }
        }

        /// <inheriteddoc/>
        public override async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            await ReceiveNotification(new ActorReminderCallback(this, reminderName, state, dueTime, period));
            await base.ReceiveReminderAsync(reminderName, state, dueTime, period);
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
            await ReceiveAndProcessQueueMessages();
            await HandleCommand(command);
        }

        /// <summary>
        /// Handles a timer callback.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual Task Handle(ActorTimerCallback timer) => ReceiveAndProcessQueueMessages();

        /// <summary>
        /// Handles a reminder callback.
        /// </summary>
        /// <param name="reminder">The reminder.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual Task Handle(ActorReminderCallback reminder) => ReceiveAndProcessQueueMessages();

        /// <summary>
        /// Handles the actor desactivation.
        /// </summary>
        /// <param name="desactivation">The desactivation.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual Task Handle(ActorDesactivation desactivation) => Task.CompletedTask;

        /// <summary>
        /// Handles the actor activation.
        /// </summary>
        /// <param name="activation">The activation.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual Task Handle(ActorActivation activation) => Task.CompletedTask;

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
            // The command '{0}' is not supported by '{1}'.
            => Task.FromException<IList<IEvent>>(new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.CommandNotSupported, command?.GetType().Name, this.ActorName())));

        /// <summary>
        /// Receives the specified event.
        /// </summary>
        /// <param name="eventMessage">The event</param>
        /// <param name="batchSave">
        /// if set to <c>true</c> do not save data. It will be done at the end of the batch.
        /// </param>
        protected virtual Task ReceiveEvent(IEvent eventMessage, bool batchSave = false)
            => Task.FromResult<IList<IEvent>>(Array.Empty<IEvent>());

        /// <summary>
        /// Receive a notification message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>List of generated events.</returns>
        protected virtual Task ReceiveNotification(IMessage message)
            => message switch
            {
                ActorActivation activation => Handle(activation),
                ActorDesactivation desactivation => Handle(desactivation),
                ActorReminderCallback reminder => Handle(reminder),
                ActorTimerCallback timer => Handle(timer),
                _ => Task.CompletedTask
            };

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        protected virtual Task<object> ReceiveQuery(IQuery query)
            => Task.FromException<object>(new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.QueryNotSupported, query?.GetType().Name, this.ActorName())));

        private async Task HandleCommand(ICommand command)
        {
            if (Id.GetId() != command.AggregateId)
            {
                // Message aggregate identifier mismatch. Expected='{0}; Message='{1}'.
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.MessageAggregateIdMismatch, Id.GetId(), command.AggregateId));
            }
            var events = await ReceiveCommand(command);
            Guid batchId = await MessageQueue.Send(events);
            foreach (IEvent anEvent in events)
            {
                await ReceiveEvent(anEvent, true);
            }
            await SetStateData();
            await MessageQueue.ConfirmSend(batchId);
            return;
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
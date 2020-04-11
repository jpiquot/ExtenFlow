using System;
using System.Collections.Generic;
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
        private readonly IMessageQueue _messageQueue;

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
            _messageQueue = messageQueue;
        }

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
            await ReceiveAndProcessMessages();
            return ReceiveQuery(query);
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
                throw new ArgumentOutOfRangeException(nameof(envelope), Properties.Resources.MessageNotCommand);
            }
            await ReceiveAndProcessMessages();
            await ReceiveMessage(message);
        }

        /// <summary>
        /// Receives the and process messages.
        /// </summary>
        public async Task ReceiveAndProcessMessages()
        {
            IMessage? message;
            while ((message = await _messageQueue.ReadNext()) != null)
            {
                IList<IEvent> events = await ReceiveMessage(message);
                await _messageQueue.Send(events);
                await _messageQueue.RemoveMessage(message.MessageId);
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
            await ReceiveAndProcessMessages();
            await _messageQueue.Send(await ReceiveCommand(command));
        }

        /// <summary>
        /// Handles a timer callback.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual Task Handle(ActorTimerCallback timer) => ReceiveAndProcessMessages();

        /// <summary>
        /// Handles a reminder callback.
        /// </summary>
        /// <param name="reminder">The reminder.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual Task Handle(ActorReminderCallback reminder) => ReceiveAndProcessMessages();

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
        protected override Task OnActivateAsync() =>
            ReceiveNotification(new ActorActivation(this));

        /// <summary>
        /// This method is called whenever an actor is deactivated after a period of inactivity.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override Task OnDeactivateAsync() =>
            ReceiveNotification(new ActorDesactivation(this));

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        protected virtual Task<IList<IEvent>> ReceiveCommand(ICommand command)
            => Task.FromException<IList<IEvent>>(new ArgumentOutOfRangeException(nameof(command)));

        /// <summary>
        /// Receive an event.
        /// </summary>
        /// <returns>List of generated events.</returns>
        protected virtual Task<IList<IEvent>> ReceiveEvent(IEvent eventMessage)
            => Task.FromException<IList<IEvent>>(new ArgumentOutOfRangeException(nameof(eventMessage)));

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
                _ => Task.FromException(new ArgumentOutOfRangeException(nameof(message)))
            };

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        protected virtual Task<object> ReceiveQuery(IQuery query)
            => Task.FromException<object>(new ArgumentOutOfRangeException(nameof(query)));

        private async Task<IList<IEvent>> ReceiveMessage(IMessage message)
        {
            if (message is IQuery query)
            {
                return new List<IEvent>(new[] { new QueryResultEvent(query, await ReceiveQuery(query)) });
            }
            if (message is ICommand command)
            {
                return await ReceiveCommand(command);
            }
            if (message is IEvent @event)
            {
                return await ReceiveEvent(@event);
            }
            await ReceiveNotification(message);
            return new List<IEvent>();
        }
    }
}
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
    public abstract class DispatchActorBase<T> : ActorBase<T>, IDispatchActor
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
        public DispatchActorBase(
            ActorService actorService,
            ActorId actorId,
            IMessageQueue messageQueue,
            IActorStateManager actorStateManager)
            : base(actorService, actorId, actorStateManager)
        {
            _messageQueue = messageQueue;
        }

        /// <summary>
        /// Asks to execute a query.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns>The query result</returns>
        public Task<object> Ask(Envelope envelope)
        {
            if (envelope == null)
            {
                throw new ArgumentNullException(nameof(envelope));
            }
            if (!(envelope.Message is IQuery query))
            {
                throw new ArgumentOutOfRangeException(nameof(envelope), Properties.Resources.MessageNotQuery);
            }
            return Execute(query);
        }

        /// <summary>
        /// Receives the and process messages.
        /// </summary>
        public async Task ReceiveAndProcessMessages()
        {
            IMessage? message;
            while ((message = await _messageQueue.ReadNext()) != null)
            {
                IList<IEvent> events = await ProcessMessage(message);
                await _messageQueue.Send(events);
                await _messageQueue.RemoveMessage(message.MessageId);
            }
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
            await _messageQueue.Send(await Execute(command));
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        protected abstract Task<IList<IEvent>> Execute(ICommand command);

        /// <summary>
        /// Executes the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        protected abstract Task<object> Execute(IQuery query);

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected abstract Task<IList<IEvent>> Handle(IMessage message);

        /// <summary>
        /// This method is called whenever an actor is activated. An actor is activated the first
        /// time any of its methods are invoked.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override Task OnActivateAsync()
        {
            // Provides opportunity to perform some optional setup.
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called whenever an actor is deactivated after a period of inactivity.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override Task OnDeactivateAsync()
        {
            // Provides Opportunity to perform optional cleanup.
            return Task.CompletedTask;
        }

        private async Task<IList<IEvent>> ProcessMessage(IMessage message)
            => message switch
            {
                ICommand command => await Execute(command),
                IQuery query => new List<IEvent>(new[] { new QueryResultEvent(query, await Execute(query)) }),
                IEvent @event => await Handle(@event),
                _ => throw new ArgumentOutOfRangeException(nameof(message))
            };
    }
}
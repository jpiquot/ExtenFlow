using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Users.Commands;
using ExtenFlow.Identity.Users.Exceptions;
using ExtenFlow.Identity.Users.Models;
using ExtenFlow.Messages;
using ExtenFlow.Messages.Dispatcher;

namespace ExtenFlow.Identity.Users.Actors
{
    /// <summary>
    /// Class NormalizedUserNameActor. Implements the <see cref="EventSourcedActorBase{String}"/>
    /// Implements the <see cref="IUserNameRegistryEntryActor"/>
    /// </summary>
    /// <seealso cref="EventSourcedActorBase{String}"/>
    /// <seealso cref="IUserNameRegistryEntryActor"/>
    public class UserNameRegistryEntryActor : EventSourcedActorBase<string>, IUserNameRegistryEntryActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="messageQueue">The message queue used to publish events.</param>
        /// <param name="eventStore">The event store used to persist events.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserNameRegistryEntryActor(
            ActorService actorService,
            ActorId actorId,
            IEventBus messageQueue,
            IEventStore eventStore,
            IActorStateManager? actorStateManager = null) : base(actorService, actorId, messageQueue, eventStore, actorStateManager)
        {
        }

        /// <summary>
        /// Creates new state.
        /// </summary>
        /// <returns>TState.</returns>
        protected override string NewState()
            => string.Empty;

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        protected override Task<IList<IEvent>> ReceiveCommand(ICommand command)
            => command switch
            {
                RegisterNormalizedUserName register => Handle(register),
                DeregisterNormalizedUserName deregister => Handle(deregister),
                _ => base.ReceiveCommand(command)
            };

        /// <summary>
        /// Receives the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="batcheSave">if set to <c>true</c> [batche save].</param>
        /// <returns></returns>
        protected override async Task ReceiveEvent(IEvent @event, bool batcheSave = false)
        {
            switch (@event)
            {
                case NormalizedUserNameRegistred registered:
                    Apply(registered);
                    break;

                case NormalizedUserNameDeregistred deregistered:
                    Apply(deregistered);
                    break;

                default:
                    await base.ReceiveEvent(@event);
                    break;
            }
        }

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        protected override async Task<object> ReceiveQuery(IQuery query)
                    => query switch
                    {
                        GetUserIdByName getId => await Handle(getId),
                        IsUserNameRegistered exist => await Handle(exist),
                        _ => Task.FromException<object>(new ArgumentOutOfRangeException(nameof(query)))
                    };

        private void Apply(NormalizedUserNameRegistred registred)
        {
            if (registred.UserId == null)
            {
                ClearState();
                return;
            }
            State = registred.UserId;
        }

        private void Apply(NormalizedUserNameDeregistred _)
            => ClearState();

        private Task<string> Handle(GetUserIdByName _)
        {
            if (StateIsNull())
            {
                throw new UserNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            return Task.FromResult(State);
        }

        private Task<bool> Handle(IsUserNameRegistered _)
        {
            if (StateIsNull())
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        private Task<IList<IEvent>> Handle(RegisterNormalizedUserName command)
        {
            if (!StateIsNull())
            {
                throw new DuplicateUserException(CultureInfo.CurrentCulture, nameof(User.NormalizedName), Id.GetId());
            }
            return Task.FromResult<IList<IEvent>>(new[] { new NormalizedUserNameRegistred(Id.GetId(), command.UserId, command.UserId, command.CorrelationId) });
        }

        private Task<IList<IEvent>> Handle(DeregisterNormalizedUserName command)
        {
            if (StateIsNull())
            {
                throw new UserNotFoundException(CultureInfo.CurrentCulture, nameof(Id), State);
            }
            return Task.FromResult<IList<IEvent>>(new[] { new NormalizedUserNameDeregistred(Id.GetId(), command.UserId, command.UserId, command.CorrelationId) });
        }
    }
}
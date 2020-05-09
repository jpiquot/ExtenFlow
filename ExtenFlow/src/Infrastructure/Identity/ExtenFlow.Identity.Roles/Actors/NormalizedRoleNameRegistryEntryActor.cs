using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Roles.Commands;
using ExtenFlow.Identity.Roles.Events;
using ExtenFlow.Identity.Roles.Exceptions;
using ExtenFlow.Identity.Roles.Models;
using ExtenFlow.Identity.Roles.Queries;
using ExtenFlow.Messages;
using ExtenFlow.Messages.Dispatcher;

namespace ExtenFlow.Identity.Roles.Actors
{
    /// <summary>
    /// Class NormalizedRoleNameActor. Implements the <see cref="EventSourcedActorBase{String}"/>
    /// Implements the <see cref="IRoleNameRegistryEntryActor"/>
    /// </summary>
    /// <seealso cref="EventSourcedActorBase{String}"/>
    /// <seealso cref="IRoleNameRegistryEntryActor"/>
    public class NormalizedRoleNameRegistryEntryActor : EventSourcedActorBase<string>, IRoleNameRegistryEntryActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="messageQueue">The message queue used to publish events.</param>
        /// <param name="eventStore">The event store used to persist events.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public NormalizedRoleNameRegistryEntryActor(
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
                RegisterNormalizedRoleName register => Handle(register),
                DeregisterNormalizedRoleName deregister => Handle(deregister),
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
                case NormalizedRoleNameRegistred registered:
                    Apply(registered);
                    break;

                case NormalizedRoleNameDeregistred deregistered:
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
                        GetRoleIdByName getId => await Handle(getId),
                        IsRoleNameRegistered exist => await Handle(exist),
                        _ => Task.FromException<object>(new ArgumentOutOfRangeException(nameof(query)))
                    };

        private void Apply(NormalizedRoleNameRegistred registred)
        {
            if (registred.RoleId == null)
            {
                ClearState();
                return;
            }
            State = registred.RoleId;
        }

        private void Apply(NormalizedRoleNameDeregistred _)
            => ClearState();

        private Task<string> Handle(GetRoleIdByName _)
        {
            if (StateIsNull())
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            return Task.FromResult(State);
        }

        private Task<bool> Handle(IsRoleNameRegistered _)
        {
            if (StateIsNull())
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        private Task<IList<IEvent>> Handle(RegisterNormalizedRoleName command)
        {
            if (!StateIsNull())
            {
                throw new DuplicateRoleException(CultureInfo.CurrentCulture, nameof(Role.NormalizedName), Id.GetId());
            }
            return Task.FromResult<IList<IEvent>>(new[] { new NormalizedRoleNameRegistred(Id.GetId(), command.RoleId, command.UserId, command.CorrelationId) });
        }

        private Task<IList<IEvent>> Handle(DeregisterNormalizedRoleName command)
        {
            if (StateIsNull())
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), State);
            }
            return Task.FromResult<IList<IEvent>>(new[] { new NormalizedRoleNameDeregistred(Id.GetId(), command.RoleId, command.UserId, command.CorrelationId) });
        }
    }
}
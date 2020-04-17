using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Properties;
using ExtenFlow.Identity.Roles.Commands;
using ExtenFlow.Identity.Roles.Events;
using ExtenFlow.Identity.Roles.Exceptions;
using ExtenFlow.Identity.Roles.Queries;
using ExtenFlow.Messages;
using ExtenFlow.Messages.Dispatcher;

namespace ExtenFlow.Identity.Roles.Actors
{
    /// <summary>
    /// The Role Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IRoleActor"/>
    public class RoleActor : EventSourcedActorBase<RoleState>, IRoleActor
    {
        private readonly IUniqueIndexActor _normalizedNameIndexActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="normalizedNameIndexActor"></param>
        /// <param name="messageQueue">The message queue used to publish events.</param>
        /// <param name="eventStore">The event store used to persist events.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleActor(
            ActorService actorService,
            ActorId actorId,
            IUniqueIndexActor normalizedNameIndexActor,
            IEventBus messageQueue,
            IEventStore eventStore,
            IActorStateManager? actorStateManager = null) : base(actorService, actorId, messageQueue, eventStore, actorStateManager)
        {
            _normalizedNameIndexActor = normalizedNameIndexActor ?? throw new ArgumentNullException(nameof(normalizedNameIndexActor));
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <returns>The role object</returns>
        public Task<Role> GetRole()
        {
            if (StateIsNull())
            {
                return Task.FromException<Role>(new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.RoleNotFound, Id.GetId())));
            }
            return Task.FromResult(
                new Role()
                {
                    Id = new Guid(Id.GetId()),
                    Name = State.Name,
                    NormalizedName = State.NormalizedName,
                    ConcurrencyStamp = State.ConcurrencyStamp
                });
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        protected override Task<IList<IEvent>> ReceiveCommand(ICommand command)
            => command switch
            {
                CreateNewRole create => Handle(create),
                DeleteRole delete => Handle(delete),
                RenameRole rename => Handle(rename),
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
                case RoleCreated create:
                    Apply(create);
                    break;

                case RoleDeleted delete:
                    Apply(delete);
                    break;

                case RoleRenamed rename:
                    Apply(rename);
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
                        GetRoleDetails create => await Handle(create),
                        _ => Task.FromException<object>(new ArgumentOutOfRangeException(nameof(query)))
                    };

        private void Apply(RoleRenamed rename)
            => State.Name = rename.Name;

        private void Apply(RoleDeleted _)
            => ClearState();

        private void Apply(RoleCreated create)
        {
            State.Name = create.Name;
            State.NormalizedName = create.NormalizedName;
        }

        private Task<RoleDetailsModel> Handle(GetRoleDetails _)
        {
            if (StateIsNull())
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            return Task.FromResult(new RoleDetailsModel(new Guid(Id.GetId()), State.Name ?? string.Empty, State.NormalizedName ?? string.Empty, State.ConcurrencyStamp));
        }

        /// <summary>
        /// Delete the role.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="RoleNotFoundException">Id</exception>
        /// <exception cref="RoleConcurrencyFailureException"></exception>
        private Task<IList<IEvent>> Handle(DeleteRole command)
        {
            if (StateIsNull())
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            if (State.ConcurrencyStamp != command.ConcurrencyStamp)
            {
                throw new RoleConcurrencyFailureException(CultureInfo.CurrentCulture, command.ConcurrencyStamp, State.ConcurrencyStamp);
            }
            return Task.FromResult<IList<IEvent>>(new[] { new RoleDeleted(Id.GetId(), command.UserId, command.CorrelationId) });
        }

        private Task<IList<IEvent>> Handle(RenameRole command)
        {
            if (StateIsNull())
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            if (State.ConcurrencyStamp != command.ConcurrencyStamp)
            {
                throw new RoleConcurrencyFailureException(CultureInfo.CurrentCulture, command.ConcurrencyStamp, State.ConcurrencyStamp);
            }
            return Task.FromResult<IList<IEvent>>(new[] { new RoleRenamed(Id.GetId(), command.Name, command.NormalizedName, command.UserId, command.CorrelationId) });
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="DuplicateRoleException">Id</exception>
        private async Task<IList<IEvent>> Handle(CreateNewRole command)
        {
            if (!StateIsNull())
            {
                throw new DuplicateRoleException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            if (await _normalizedNameIndexActor.Exist(command.NormalizedName))
            {
                throw new DuplicateRoleException(CultureInfo.CurrentCulture, nameof(RoleState.NormalizedName), command.NormalizedName);
            }
            return new[] { new RoleCreated(Id.GetId(), command.Name, command.NormalizedName, command.UserId, command.CorrelationId) };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Properties;
using ExtenFlow.Messages;
using ExtenFlow.Messages.Dispatcher;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Roles
{
    /// <summary>
    /// The Role Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IRoleActor"/>
    public class RoleActor : DispatchActorBase<Role>, IRoleActor
    {
        private readonly IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="messageQueue"></param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleActor(
            ActorService actorService,
            ActorId actorId,
            IEventBus messageQueue,
            IActorStateManager? actorStateManager = null) : base(actorService, actorId, messageQueue, actorStateManager)
        {
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <returns>The role object</returns>
        public Task<Role> GetRole()
        {
            if (State == null || State.Id == default)
            {
                return Task.FromException<Role>(new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.RoleNotFound, Id.GetId())));
            }
            return Task.FromResult<Role>(State);
        }

        /// <summary>
        /// Updates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <exception cref="ArgumentNullException">Role.Id</exception>
        /// <returns>The identity result object</returns>
        public async Task<IdentityResult> SetRole(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (role.Id == default)
            {
                throw new ArgumentOutOfRangeException(Resources.RoleIdNotDefined);
            }
            if (State?.ConcurrencyStamp != null && role.ConcurrencyStamp != State.ConcurrencyStamp)
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            if (State == null || State.Id == default)
            {
                // Create an new role
                await _collectionService.Add(Id.GetId());
            }
            State = role;
            await SetStateData();
            return IdentityResult.Success;
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        protected override async Task<IList<IEvent>> ReceiveCommand(ICommand command)
            => command switch
            {
                CreateNewRole create => Handle(create),
                DeleteRole delete => Handle(delete),
                RenameRole rename => Handle(rename),
                ChangeRoleNormalizedName changeNormalizedName => Handle(changeNormalizedName),
                _ => await base.ReceiveCommand(command)
            };

        /// <summary>
        /// Receives the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="batcheSave">if set to <c>true</c> [batche save].</param>
        /// <returns></returns>
        protected override async Task ReceiveEvent(IEvent @event, bool batcheSave = false)
            => @event switch
            {
                RoleCreated create => Apply(create),
                RoleDeleted delete => Apply(delete),
                RoleRenamed rename => Apply(rename),
                RoleNormalizedNameChanged changeNormalizedName => Apply(changeNormalizedName),
                _ => await base.ReceiveEvent(@event)
            };

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        protected override Task<object> ReceiveQuery(IQuery query)
                    => query switch
                    {
                        RoleCreated create => Handle(create),
                        RoleDeleted delete => Handle(delete),
                        RoleRenamed rename => Handle(rename),
                        RoleNormalizedNameChanged changeNormalizedName => Handle(changeNormalizedName),
                        _ => Task.FromException<object>(new ArgumentOutOfRangeException(nameof(query)))
                    };

        private void Apply(RoleNormalizedNameChanged changeNormalizedName)
        {
            if (State != null)
            {
            }
        }

        private void Apply(RoleRenamed rename)
        {
        }

        private void Apply(RoleDeleted delete)
        {
        }

        private void Apply(RoleCreated create)
        {
        }

        /// <summary>
        /// Delete the role.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="ExtenFlow.Identity.Roles.RoleNotFoundException">Id</exception>
        /// <exception cref="ExtenFlow.Identity.Roles.RoleConcurrencyFailureException"></exception>
        private IList<IEvent> Handle(DeleteRole command)
        {
            if (State == null)
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            if (State.ConcurrencyStamp != command.ConcurrencyStamp)
            {
                throw new RoleConcurrencyFailureException(CultureInfo.CurrentCulture, command.ConcurrencyStamp, State.ConcurrencyStamp);
            }
            return new[] { new RoleDeleted(Id.GetId(), command.UserId, command.CorrelationId) };
        }

        private IList<IEvent> Handle(RenameRole command)
        {
            if (State == null)
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            if (State.ConcurrencyStamp != command.ConcurrencyStamp)
            {
                throw new RoleConcurrencyFailureException(CultureInfo.CurrentCulture, command.ConcurrencyStamp, State.ConcurrencyStamp);
            }
            return new[] { new RoleRenamed(Id.GetId(), command.Name, command.UserId, command.CorrelationId) };
        }

        private IList<IEvent> Handle(ChangeRoleNormalizedName command)
        {
            if (State == null)
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            if (State.ConcurrencyStamp != command.ConcurrencyStamp)
            {
                throw new RoleConcurrencyFailureException(CultureInfo.CurrentCulture, command.ConcurrencyStamp, State.ConcurrencyStamp);
            }
            return new[] { new RoleRenamed(Id.GetId(), command.NormalizedName, command.UserId, command.CorrelationId) };
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="DuplicateRoleException">Id</exception>
        private IList<IEvent> Handle(CreateNewRole command)
        {
            if (State != null)
            {
                throw new DuplicateRoleException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            return new[] { new RoleCreated(Id.GetId(), command.Name, command.NormalizedName, command.UserId, command.CorrelationId) };
        }
    }
}
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

namespace ExtenFlow.Identity.Actors
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
            IMessageQueue messageQueue,
            IActorStateManager? actorStateManager = null) : base(actorService, actorId, messageQueue, actorStateManager)
        {
        }

        /// <summary>
        /// Delete the role.
        /// </summary>
        /// <param name="concurrencyString">The concurrency string.</param>
        /// <returns>The identity result object</returns>
        public async Task<IdentityResult> DeleteRole(string concurrencyString)
        {
            if (State == null)
            {
                return IdentityResult.Success;
            }
            if (State.ConcurrencyStamp != concurrencyString)
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            var state = State;
            State = null;
            await SetStateData();
            return IdentityResult.Success;
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

        protected override Task<IList<IEvent>> Execute(ICommand command)
            => command switch
            {
                CreateNewRole create => Handle(create),
                DeleteRole delete => Handle(delete),
                RenameRole rename => Handle(rename),
                ChangeRoleNormalizedName changeNormalizedName => Handle(changeNormalizedName),
                _ => Task.FromException<IList<IEvent>>(new ArgumentOutOfRangeException(nameof(command)))
            };

        protected override Task<object> Execute(IQuery query)
            => query switch
            {
                RoleCreated create => Handle(create),
                RoleDeleted delete => Handle(delete),
                RoleRenamed rename => Handle(rename),
                RoleNormalizedNameChanged changeNormalizedName => Handle(changeNormalizedName),
                _ => Task.FromException<object>(new ArgumentOutOfRangeException(nameof(query)))
            };

        protected override Task<IList<IEvent>> Handle(IMessage message)
        {
        }
    }
}
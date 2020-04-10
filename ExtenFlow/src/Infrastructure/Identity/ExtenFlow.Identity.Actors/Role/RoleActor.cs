using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Identity.Properties;
using ExtenFlow.Identity.Services;
using ExtenFlow.Messages;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The User Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IRoleActor"/>
    public class RoleActor : DispatchActorBase<Role>, IRoleActor
    {
        private readonly IRoleCollectionService _collectionService;
        private readonly IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();
        private readonly IRoleNameIndexService _nameService;
        private readonly IRoleNormalizedNameIndexService _normalizedNameService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="collectionService">The collection service to maintain the list of roles</param>
        /// <param name="nameService">Name indexer service</param>
        /// <param name="normalizedNameService">Normalized name indexer</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleActor(
            ActorService actorService,
            ActorId actorId,
            IRoleCollectionService collectionService,
            IRoleNameIndexService nameService,
            IRoleNormalizedNameIndexService normalizedNameService,
            IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
            _collectionService = collectionService;
            _nameService = nameService;
            _normalizedNameService = normalizedNameService;
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
            await _nameService.Remove(state.Name);
            await _normalizedNameService.Remove(state.NormalizedName);
            await _collectionService.Remove(Id.GetId());
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

        protected override Task<IList<IEvent>> Execute(ICommand command) => throw new NotImplementedException();

        protected override Task<object> Execute(IQuery query) => throw new NotImplementedException();

        protected override Task<IList<IEvent>> Handle(IMessage message) => throw new NotImplementedException();
    }
}
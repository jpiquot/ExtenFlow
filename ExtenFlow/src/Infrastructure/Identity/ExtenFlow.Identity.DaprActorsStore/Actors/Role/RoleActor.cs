using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The User Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IRoleActor"/>
    public class RoleActor : Actor, IRoleActor
    {
        private const string _stateName = "Role";
        private Role? _state;
        private IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        private Role State => _state ?? throw new NullReferenceException(nameof(_state));

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <returns>The role object</returns>
        public Task<Role> GetRole() => Task.FromResult(State);

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<Role?>(_stateName);
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Updates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <exception cref="ArgumentNullException">Role.Id</exception>
        /// <returns>The identity result object</returns>
        public async Task<IdentityResult> Update(Role role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.Id))
            {
                throw new ArgumentNullException(nameof(Role) + "." + nameof(Role.Id));
            }
            if (_state != null && !role.ConcurrencyStamp.Equals(State.ConcurrencyStamp))
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            role.NormalizedName = role.Id;
            await StateManager.SetStateAsync<Role?>(_stateName, role);
            _state = role;
            return IdentityResult.Success;
        }

        /// <summary>
        /// Clears the specified concurrency string.
        /// </summary>
        /// <param name="concurrencyString">The concurrency string.</param>
        /// <returns>The identity result object</returns>
        public async Task<IdentityResult> Clear(string concurrencyString)
        {
            if (_state == null)
            {
                throw new KeyNotFoundException("The role does not exist.");
            }
            if (!State.ConcurrencyStamp.Equals(concurrencyString))
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            await StateManager.SetStateAsync<Role?>(_stateName, null);
            _state = null;
            return IdentityResult.Success;
        }
    }
}
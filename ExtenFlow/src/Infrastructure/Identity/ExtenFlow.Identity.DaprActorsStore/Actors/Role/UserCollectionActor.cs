using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The role collection actor class
    /// </summary>
    public class RoleCollectionActor : Actor, IRoleCollectionActor
    {
        private const string _stateName = "RoleCollection";
        private readonly IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleCollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        private HashSet<string>? _state;

        private IRoleActor GetRoleActor(string roleId) => ActorProxy.Create<IRoleActor>(new ActorId(roleId), nameof(RoleActor));

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="role">The new role properties</param>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Create(Role role)
        {
            if (string.IsNullOrWhiteSpace(role?.Id))
            {
                throw new ArgumentNullException(nameof(Role.Id));
            }
            if (_state == null)
            {
                _state = new HashSet<string>();
            }
            _state.TryGetValue(role.Id, out string? value);
            if (value != null)
            {
                return IdentityResult.Failed(_errorDescriber.DuplicateRoleName(value));
            }
            _state.Add(role.Id);
            IdentityResult result;
            try
            {
                result = await GetRoleActor(role.Id).Update(role);
            }
            catch (Exception e)
            {
                _state.Remove(role.Id);
                throw e;
            }
            if (!result.Succeeded)
            {
                _state.Remove(role.Id);
            }
            await StateManager.SetStateAsync(_stateName, _state);
            return result;
        }

        /// <summary>
        /// Checks if a role with the given identifier exists.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>true if the role exists, else false.</returns>
        public Task<bool> Exist(string roleId)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(roleId)));
            }
            if (_state == null || _state.Count < 1)
            {
                return Task.FromResult(false);
            }
            _state.TryGetValue(roleId, out string? value);
            return Task.FromResult(value != null);
        }

        /// <summary>
        /// Delete the role
        /// </summary>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Delete(string roleId, string concurrencyString)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new ArgumentNullException(nameof(roleId));
            }
            string? value = null;
            _state?.TryGetValue(roleId, out value);
            if (_state == null || value == null)
            {
                return IdentityResult.Failed(_errorDescriber.InvalidRoleName(roleId));
            }
            IdentityResult result = await GetRoleActor(roleId).Clear(concurrencyString);
            if (result.Succeeded)
            {
                _state.Remove(value);
            }
            await StateManager.SetStateAsync(_stateName, _state);
            return result;
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task">Task</see> that represents outstanding
        /// OnActivateAsync operation.
        /// </returns>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<HashSet<string>?>(_stateName);
            await base.OnActivateAsync();
        }
    }
}
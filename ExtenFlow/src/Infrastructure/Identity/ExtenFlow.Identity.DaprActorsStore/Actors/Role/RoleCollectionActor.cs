using System;
using System.Linq;
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
        /// Initializes a new instance of the <see cref="RoleCollectionActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleCollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        private RoleCollectionState? _state;
        private RoleCollectionState State => _state ?? (_state = new RoleCollectionState());

        private IRoleActor GetRoleActor(Guid roleId) => ActorProxy.Create<IRoleActor>(new ActorId(roleId.ToString()), nameof(RoleActor));

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="role">The new role properties</param>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Create(Role role)
        {
            if (role == null || role.Id == default)
            {
                throw new ArgumentNullException(nameof(Role.Id));
            }
            if (State.Ids.Any(p => p == role.Id))
            {
                throw new InvalidOperationException($"The role with Id='{role.Id}' already exist.");
            }
            if (State.NormalizedNames.Any(p => p.Key.Equals(role.NormalizedName)))
            {
                return IdentityResult.Failed(_errorDescriber.DuplicateRoleName(role.NormalizedName));
            }
            IdentityResult result = await GetRoleActor(role.Id).Set(role);
            if (result.Succeeded)
            {
                State.Ids.Add(role.Id);
                State.NormalizedNames.Add(role.NormalizedName, role.Id);
                await StateManager.SetStateAsync(_stateName, _state);
            }
            return result;
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="role">The new role properties</param>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Update(Role role)
        {
            if (role == null || role.Id == default)
            {
                throw new ArgumentNullException(nameof(Role.Id));
            }
            if (!State.Ids.Any(p => p == role.Id))
            {
                throw new InvalidOperationException($"The role with Id='{role.Id}' does not exist.");
            }
            if (State.NormalizedNames.Any(p => p.Key.Equals(role.NormalizedName) && p.Value != role.Id))
            {
                return IdentityResult.Failed(_errorDescriber.DuplicateRoleName(role.NormalizedName));
            }
            IdentityResult result = await GetRoleActor(role.Id).Set(role);
            if (result.Succeeded)
            {
                if (!State.NormalizedNames.Any(p => p.Key.Equals(role.NormalizedName)))
                {
                    // The normalized name hase been changed.
                    State.NormalizedNames.Remove(State.NormalizedNames.Where(p => p.Value == role.Id).Select(p => p.Key).Single());
                    State.NormalizedNames.Add(role.NormalizedName, role.Id);
                }
                await StateManager.SetStateAsync(_stateName, _state);
            }
            return result;
        }

        /// <summary>
        /// Checks if a role with the given identifier exists.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>true if the role exists, else false.</returns>
        public Task<bool> Exist(Guid roleId)
        {
            if (roleId == default)
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(roleId)));
            }
            if (_state == null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(State.Ids.Any(p => p == roleId));
        }

        /// <summary>
        /// Delete the role
        /// </summary>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Delete(Guid roleId, string concurrencyString)
        {
            if (roleId == default)
            {
                throw new ArgumentNullException(nameof(roleId));
            }
            if (!State.Ids.Any(p => p == roleId))
            {
                throw new InvalidOperationException($"The role with Id='{roleId}' does not exist.");
            }
            State.NormalizedNames.Remove(State.NormalizedNames.Where(p => p.Value == roleId).Select(p => p.Key).Single());
            State.Ids.Remove(roleId);
            await StateManager.SetStateAsync(_stateName, _state);
            await GetRoleActor(roleId).Clear(concurrencyString);
            return IdentityResult.Success;
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
            _state = await StateManager.GetStateAsync<RoleCollectionState?>(_stateName);
            await base.OnActivateAsync();
        }
    }
}
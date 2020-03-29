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
    /// The user collection actor class
    /// </summary>
    public class UserCollectionActor : Actor, IUserCollectionActor
    {
        private const string _stateName = "UserCollection";
        private readonly IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserCollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        private HashSet<string>? _state;

        private IUserActor GetUserActor(string userId) => ActorProxy.Create<IUserActor>(new ActorId(userId), nameof(UserActor));

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Create(User user)
        {
            if (string.IsNullOrWhiteSpace(user?.Id))
            {
                throw new ArgumentNullException(nameof(User.Id));
            }
            if (_state == null)
            {
                _state = new HashSet<string>();
            }
            _state.TryGetValue(user.Id, out string? value);
            if (value != null)
            {
                return IdentityResult.Failed(_errorDescriber.DuplicateUserName(value));
            }
            _state.Add(user.Id);
            IdentityResult result;
            try
            {
                result = await GetUserActor(user.Id).Create(user);
            }
            catch (Exception e)
            {
                _state.Remove(user.Id);
                throw e;
            }
            if (!result.Succeeded)
            {
                _state.Remove(user.Id);
            }
            await StateManager.SetStateAsync(_stateName, _state);
            return result;
        }

        /// <summary>
        /// Checks if a user with the given identifier exists.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if the user exists, else false.</returns>
        public Task<bool> Exist(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(userId)));
            }
            if (_state == null || _state.Count < 1)
            {
                return Task.FromResult(false);
            }
            _state.TryGetValue(userId, out string? value);
            return Task.FromResult(value != null);
        }

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Delete(string userId, string concurrencyString)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            string? value = null;
            _state?.TryGetValue(userId, out value);
            if (_state == null || value == null)
            {
                return IdentityResult.Failed(_errorDescriber.InvalidUserName(userId));
            }
            IdentityResult result = await GetUserActor(userId).Delete(concurrencyString);
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
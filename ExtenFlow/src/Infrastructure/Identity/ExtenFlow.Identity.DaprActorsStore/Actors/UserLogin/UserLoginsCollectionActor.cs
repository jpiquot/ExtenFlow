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
    /// The user collection actor class
    /// </summary>
    public class UserLoginsCollectionActor : Actor, IUserLoginsCollectionActor
    {
        private const string _stateName = "UserLoginsCollection";
        private readonly IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCollectionActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserCollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        private UserCollectionState? _state;
        private UserCollectionState State => _state ?? (_state = new UserCollectionState());

        private IUserActor GetUserActor(Guid userId) => ActorProxy.Create<IUserActor>(new ActorId(userId.ToString()), nameof(UserActor));

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Create(User user)
        {
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(User.Id));
            }
            if (State.Ids.Any(p => p == user.Id))
            {
                throw new InvalidOperationException($"The user with Id='{user.Id}' already exist.");
            }
            if (State.NormalizedNames.Any(p => p.Key.Equals(user.NormalizedUserName)))
            {
                return IdentityResult.Failed(_errorDescriber.DuplicateUserName(user.NormalizedUserName));
            }
            IdentityResult result = await GetUserActor(user.Id).Set(user);
            if (result.Succeeded)
            {
                State.Ids.Add(user.Id);
                State.NormalizedNames.Add(user.NormalizedUserName, user.Id);
                await StateManager.SetStateAsync(_stateName, _state);
            }
            return result;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Update(User user)
        {
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(User.Id));
            }
            if (!State.Ids.Any(p => p == user.Id))
            {
                throw new InvalidOperationException($"The user with Id='{user.Id}' does not exist.");
            }
            if (State.NormalizedNames.Any(p => p.Key.Equals(user.NormalizedUserName) && p.Value != user.Id))
            {
                return IdentityResult.Failed(_errorDescriber.DuplicateUserName(user.NormalizedUserName));
            }
            IdentityResult result = await GetUserActor(user.Id).Set(user);
            if (result.Succeeded)
            {
                if (!State.NormalizedNames.Any(p => p.Key.Equals(user.NormalizedUserName)))
                {
                    // The normalized name hase been changed.
                    State.NormalizedNames.Remove(State.NormalizedNames.Where(p => p.Value == user.Id).Select(p => p.Key).Single());
                    State.NormalizedNames.Add(user.NormalizedUserName, user.Id);
                }
                await StateManager.SetStateAsync(_stateName, _state);
            }
            return result;
        }

        /// <summary>
        /// Checks if a user with the given identifier exists.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if the user exists, else false.</returns>
        public Task<bool> Exist(Guid userId)
        {
            if (userId == default)
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(userId)));
            }
            if (_state == null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(State.Ids.Any(p => p == userId));
        }

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Delete(Guid userId, string concurrencyString)
        {
            if (userId == default)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (!State.Ids.Any(p => p == userId))
            {
                throw new InvalidOperationException($"The user with Id='{userId}' does not exist.");
            }
            State.NormalizedNames.Remove(State.NormalizedNames.Where(p => p.Value == userId).Select(p => p.Key).Single());
            State.Ids.Remove(userId);
            await StateManager.SetStateAsync(_stateName, _state);
            await GetUserActor(userId).Clear(concurrencyString);
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
            _state = await StateManager.GetStateAsync<UserCollectionState?>(_stateName);
            await base.OnActivateAsync();
        }
    }
}
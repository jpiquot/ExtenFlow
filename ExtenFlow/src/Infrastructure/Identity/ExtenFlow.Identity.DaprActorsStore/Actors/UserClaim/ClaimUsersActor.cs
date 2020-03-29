using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The claim's users class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IClaimUsersActor"/>
    public class ClaimUsersActor : Actor, IClaimUsersActor
    {
        private const string _stateName = "ClaimUsers";
        private Dictionary<string, string>? _state;

        private Dictionary<string, string> State => _state ?? (_state = new Dictionary<string, string>());

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimUsersActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public ClaimUsersActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<Dictionary<string, string>?>(_stateName);
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Determines whether the claim has the specified user.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name</param>
        /// <param name="claimValue">The claim value</param>
        /// <returns>True if the claim has the user</returns>
        /// <exception cref="ArgumentNullException">user normalized name</exception>
        public Task<bool> HasUser(string userNormalizedName, string claimValue)
        {
            if (string.IsNullOrWhiteSpace(userNormalizedName))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(userNormalizedName)));
            }
            return Task.FromResult(State.Where(p => p.Key.Equals(userNormalizedName) && p.Value.Equals(claimValue)).Any());
        }

        /// <summary>
        /// Adds the claim's users.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name</param>
        /// <param name="claimValue">The claim value</param>
        /// <exception cref="ArgumentNullException">user normalized name</exception>
        public Task AddUser(string userNormalizedName, string claimValue)
        {
            if (string.IsNullOrWhiteSpace(userNormalizedName))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(userNormalizedName)));
            }
            if (State.Where(p => p.Key.Equals(userNormalizedName) && p.Value.Equals(claimValue)).Any())
            {
                return Task.FromException(new InvalidOperationException($"The user '{userNormalizedName}, has already the claim '{Id.GetId()}'."));
            }
            State.Add(userNormalizedName, claimValue);
            return StateManager.SetStateAsync(_stateName, _state);
        }

        /// <summary>
        /// Removes the claims.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name</param>
        /// <param name="claimValue">The claim value</param>
        /// <exception cref="ArgumentNullException">user normalized name</exception>
        public Task RemoveUser(string userNormalizedName, string claimValue)
        {
            if (string.IsNullOrWhiteSpace(userNormalizedName))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(userNormalizedName)));
            }
            if (!State.Where(p => p.Key.Equals(userNormalizedName) && p.Value.Equals(claimValue)).Any())
            {
                return Task.FromException(new InvalidOperationException($"The user '{userNormalizedName}, does not have the claim '{Id.GetId()}'."));
            }
            State.Remove(userNormalizedName);
            return StateManager.SetStateAsync(_stateName, _state);
        }

        /// <summary>
        /// Gets the claim values for a user.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name.</param>
        /// <exception cref="ArgumentNullException">user normalized name</exception>
        /// <returns>A list of all claim values for this user</returns>
        public Task<IList<string>> GetClaimValues(string userNormalizedName)
        {
            if (string.IsNullOrWhiteSpace(userNormalizedName))
            {
                return Task.FromException<IList<string>>(new ArgumentNullException(nameof(userNormalizedName)));
            }
            return Task.FromResult<IList<string>>(State.Where(p => p.Key.Equals(userNormalizedName)).Select(p => p.Value).ToList());
        }
    }
}
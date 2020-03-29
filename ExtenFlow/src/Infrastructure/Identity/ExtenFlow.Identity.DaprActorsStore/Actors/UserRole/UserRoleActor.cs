using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user roles class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserRolesActor"/>
    public class UserRolesActor : Actor, IUserRolesActor
    {
        private const string _stateName = "UserRoles";
        private HashSet<string>? _state;
        private IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        private HashSet<string> State => _state ?? (_state = new HashSet<string>());

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserRolesActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<HashSet<string>>(_stateName);
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Determines whether the user has the specified role.
        /// </summary>
        /// <param name="roleNormalizedName">Normalized name of the role.</param>
        /// <returns>True if the user has the role</returns>
        /// <exception cref="ArgumentNullException">roleNormalizedName</exception>
        public Task<bool> HasRole(string roleNormalizedName)
        {
            if (string.IsNullOrWhiteSpace(roleNormalizedName))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(roleNormalizedName)));
            }
            return Task.FromResult(State.Where(p => p.Equals(roleNormalizedName)).Any());
        }

        /// <summary>
        /// Adds the user's role.
        /// </summary>
        /// <param name="roleNormalizedName">Name of the role normalized.</param>
        /// <exception cref="ArgumentNullException">roleNormalizedName</exception>
        public Task AddRole(string roleNormalizedName)
        {
            if (State.Where(p => p.Equals(roleNormalizedName)).Any())
            {
                return Task.FromException(new InvalidOperationException($"The user is already in role '{roleNormalizedName}'."));
            }
            State.Add(roleNormalizedName);
            return StateManager.SetStateAsync(_stateName, _state);
        }

        /// <summary>
        /// Removes the role.
        /// </summary>
        /// <param name="roleNormalizedName">Name of the role normalized.</param>
        /// <exception cref="ArgumentNullException">roleNormalizedName</exception>
        public Task RemoveRole(string roleNormalizedName)
        {
            if (!State.Where(p => p.Equals(roleNormalizedName)).Any())
            {
                return Task.FromException(new InvalidOperationException($"The user does not have the role '{roleNormalizedName}'."));
            }
            State.Remove(roleNormalizedName);
            return StateManager.SetStateAsync(_stateName, _state);
        }

        /// <summary>
        /// Gets the all the user's roles.
        /// </summary>
        /// <returns>A list of all roles</returns>
        public Task<IList<string>> GetRoles() => Task.FromResult<IList<string>>(State.ToList());
    }
}
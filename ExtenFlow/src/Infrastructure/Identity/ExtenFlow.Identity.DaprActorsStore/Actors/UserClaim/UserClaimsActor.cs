using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user claims class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserClaimsActor"/>
    public class UserClaimsActor : Actor, IUserClaimsActor
    {
        private const string _stateName = "UserClaims";
        private HashSet<string>? _state;

        private HashSet<string> State => _state ?? (_state = new HashSet<string>());

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimsActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserClaimsActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        private IClaimUsersActor GetClaimUsersActor(string claimType) => ActorProxy.Create<IClaimUsersActor>(new ActorId(claimType), nameof(ClaimUsersActor));

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<HashSet<string>?>(_stateName);
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Determines whether the user has the specified Claim.
        /// </summary>
        /// <param name="claimType">Type of the claim</param>
        /// <param name="claimValue">Value of the claim</param>
        /// <returns>True if the user has the Claim</returns>
        /// <exception cref="ArgumentNullException">claimType</exception>
        public Task<bool> HasClaim(string claimType, string claimValue)
        {
            if (string.IsNullOrWhiteSpace(claimType))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(claimType)));
            }
            if (!State.Any(p => p.Equals(claimType)))
            {
                return Task.FromResult(false);
            }
            return GetClaimUsersActor(claimType).HasUser(Id.GetId(), claimValue);
        }

        /// <summary>
        /// Adds the user's Claim.
        /// </summary>
        /// <param name="claimType">Type of the claim</param>
        /// <param name="claimValue">Value of the claim</param>
        /// <exception cref="ArgumentNullException">claimType</exception>
        public async Task AddClaim(string claimType, string claimValue)
        {
            if (State.Where(p => p.Equals(claimType)).Any())
            {
                throw new InvalidOperationException($"The user is already in Claim '{claimType}'.");
            }
            State.Add(claimType);
            await StateManager.SetStateAsync(_stateName, _state);
            await GetClaimUsersActor(claimType).AddUser(Id.GetId(), claimValue);
        }

        /// <summary>
        /// Removes the Claim.
        /// </summary>
        /// <param name="claimType">Type of the claim</param>
        /// <param name="claimValue">Value of the claim</param>
        /// <exception cref="ArgumentNullException">claimType</exception>
        public async Task RemoveClaim(string claimType, string claimValue)
        {
            if (!State.Where(p => p.Equals(claimType)).Any())
            {
                throw new InvalidOperationException($"The user does not have the Claim '{claimType}'.");
            }
            await GetClaimUsersActor(claimType).RemoveUser(Id.GetId(), claimValue);
            State.Remove(claimType);
            await StateManager.SetStateAsync(_stateName, _state);
        }

        /// <summary>
        /// Gets the all the user's Claims.
        /// </summary>
        /// <returns>A list of all Claims</returns>
        public async Task<IList<Tuple<string, string>>> GetClaims()
        {
            var claims = new List<Tuple<string, string>>();
            foreach (string claimType in State)
            {
                foreach (string claimValue in await GetClaimUsersActor(claimType).GetClaimValues(Id.GetId()))
                {
                    claims.Add(new Tuple<string, string>(claimType, claimValue));
                }
            }
            return claims;
        }
    }
}
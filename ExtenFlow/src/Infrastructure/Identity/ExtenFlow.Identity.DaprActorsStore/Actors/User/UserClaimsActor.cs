using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The User Claims Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserClaimsActor"/>
    public class UserClaimsActor : Actor, IUserClaimsActor
    {
        private const string _stateName = "UserClaim";
        private Dictionary<string, Claim>? _state;
        private Dictionary<string, Claim> State => _state ?? (_state = new Dictionary<string, Claim>());

        /// <summary>
        /// Initializes a new instance of the user claims actor ( <see cref="UserClaimsActor"/>) class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserClaimsActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<Dictionary<string, Claim>?>(_stateName);
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Gets the claims.
        /// </summary>
        /// <returns>The list of user's claims</returns>
        public Task<IList<Claim>> GetClaims() => Task.FromResult<IList<Claim>>(State.Values.ToList());

        /// <summary>
        /// Adds the claims to the user.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">The claim type is not defined.</exception>
        public Task AddClaims(IEnumerable<Claim> claims)
        {
            foreach (Claim claim in claims)
            {
                if (string.IsNullOrWhiteSpace(claim.Type))
                {
                    throw new NotSupportedException("The claim type is not defined.");
                }
                if (!State.TryAdd(claim.Type, claim))
                {
                    throw new InvalidOperationException("Duplicate claim type : " + claim.Type);
                }
            }
            return StateManager.SetStateAsync(_stateName, _state);
        }

        /// <summary>
        /// Replaces the claim.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="newClaim">The new claim.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">The claim type is not defined.</exception>
        public Task ReplaceClaim(string type, Claim newClaim)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new NotSupportedException("The claim type is not defined.");
            }
            State[type] = newClaim;
            return StateManager.SetStateAsync(_stateName, _state);
        }

        /// <summary>
        /// Removes the claims.
        /// </summary>
        /// <param name="claimTypes">The claim types.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">The claim type is not defined.</exception>
        public Task RemoveClaims(string[] claimTypes)
        {
            foreach (string claim in claimTypes)
            {
                if (string.IsNullOrWhiteSpace(claim))
                {
                    throw new NotSupportedException("The claim type is not defined.");
                }
                State.Remove(claim);
            }
            return StateManager.SetStateAsync(_stateName, _state);
        }
    }
}
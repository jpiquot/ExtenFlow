using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The role claims class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IRoleClaimsActor"/>
    public class RoleClaimsActor : ActorBase<Dictionary<string, HashSet<string>>>, IRoleClaimsActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimsActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleClaimsActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Adds the role's Claim.
        /// </summary>
        /// <param name="claimType">Type of the claim</param>
        /// <param name="claimValue">Value of the claim</param>
        /// <exception cref="ArgumentNullException">claimType</exception>
        public Task Add(string claimType, string claimValue)
        {
            if (string.IsNullOrWhiteSpace(claimType))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(claimType)));
            }
            if (State == null)
            {
                State = new Dictionary<string, HashSet<string>>();
            }
            ClaimValues(claimType).Add(claimValue);
            return SetStateData();
        }

        /// <summary>
        /// Determines whether the role has the specified Claim.
        /// </summary>
        /// <param name="claimType">Type of the claim</param>
        /// <param name="claimValue">Value of the claim</param>
        /// <returns>True if the role has the Claim</returns>
        /// <exception cref="ArgumentNullException">claimType</exception>
        public Task<bool> Exist(string claimType, string claimValue)
        {
            if (string.IsNullOrWhiteSpace(claimType))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(claimType)));
            }
            if (State == null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(ClaimValues(claimType).Any(p => p == claimValue));
        }

        /// <summary>
        /// Gets the all the role's Claims.
        /// </summary>
        /// <returns>A list of all Claims</returns>
        public Task<IList<Tuple<string, string>>> GetAll()
        {
            var list = new List<Tuple<string, string>>();
            if (State != null)
            {
                foreach (KeyValuePair<string, HashSet<string>> entry in State)
                {
                    foreach (string value in entry.Value)
                    {
                        list.Add(new Tuple<string, string>(entry.Key, value));
                    }
                }
            }
            return Task.FromResult<IList<Tuple<string, string>>>(list);
        }

        /// <summary>
        /// Removes the Claim.
        /// </summary>
        /// <param name="claimType">Type of the claim</param>
        /// <param name="claimValue">Value of the claim</param>
        /// <exception cref="ArgumentNullException">claimType</exception>
        public Task Remove(string claimType, string claimValue)
        {
            if (string.IsNullOrWhiteSpace(claimType))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(claimType)));
            }
            if (State == null)
            {
                return Task.CompletedTask;
            }
            ClaimValues(claimType).Remove(claimValue);
            return SetStateData();
        }

        private HashSet<string> ClaimValues(string claimType)
        {
            if (State == null)
            {
                // The actor state has not been initialized for actor {0} with Id '{1}'.
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExtenFlow.Actors.Properties.Resources.ActorStateNotInitialized, this.ActorName(), Id.GetId()));
            }
            if (!State.TryGetValue(claimType, out HashSet<string>? values))
            {
                values = new HashSet<string>();
                State.Add(claimType, values);
            }
            return values;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Roles.Domain
{
    /// <summary>
    /// The role claims class
    /// </summary>
    /// <seealso cref="Actor"/>
    public class RoleClaimsEntity : EntityState<Dictionary<string, HashSet<string>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimsEntity"/> class.
        /// </summary>
        /// <param name="actorStateManager">The aggregate root actor state manager.</param>
        public RoleClaimsEntity(IActorStateManager actorStateManager)
            : base("Claims", actorStateManager, new Dictionary<string, HashSet<string>>())
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
            ClaimValues(claimType).Add(claimValue);
            return Save();
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
            return Task.FromResult(ClaimValues(claimType).Any(p => p == claimValue));
        }

        /// <summary>
        /// Gets the all the role's Claims.
        /// </summary>
        /// <returns>A list of all Claims</returns>
        public Task<IList<Tuple<string, string>>> GetAll()
        {
            var list = new List<Tuple<string, string>>();
            foreach (KeyValuePair<string, HashSet<string>> entry in State)
            {
                foreach (string value in entry.Value)
                {
                    list.Add(new Tuple<string, string>(entry.Key, value));
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
            return Save();
        }

        private HashSet<string> ClaimValues(string claimType)
        {
            if (!State.TryGetValue(claimType, out HashSet<string>? values))
            {
                return new HashSet<string>();
            }
            return values;
        }
    }
}
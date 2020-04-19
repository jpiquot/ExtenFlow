using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.EventStorage;
using ExtenFlow.Messages.Dispatcher;

namespace ExtenFlow.Identity.Users.Actors
{
    /// <summary>
    /// The user claims class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserClaimsActor"/>
    public class UserClaimsActor : EventSourcedActorBase<UserClaimsState>, IUserClaimsActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimsActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="eventBus"></param>
        /// <param name="eventStore"></param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserClaimsActor(ActorService actorService, ActorId actorId, IEventBus eventBus, IEventStore eventStore, IActorStateManager? actorStateManager = null)
            : base(actorService, actorId, eventBus, eventStore, actorStateManager)
        {
        }

        /// <summary>
        /// Adds the user's Claim.
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
            return SetStateData();
        }

        /// <summary>
        /// Determines whether the user has the specified Claim.
        /// </summary>
        /// <param name="claimType">Type of the claim</param>
        /// <param name="claimValue">Value of the claim</param>
        /// <returns>True if the user has the Claim</returns>
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
        /// Gets the all the user's Claims.
        /// </summary>
        /// <returns>A list of all Claims</returns>
        public Task<IList<Tuple<string, string>>> GetAll()
        {
            var list = new List<Tuple<string, string>>();
            if (State != null)
            {
                foreach (KeyValuePair<string, HashSet<string>> entry in State.Claims)
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
            if (!State.Claims.TryGetValue(claimType, out HashSet<string>? values))
            {
                values = new HashSet<string>();
                State.Claims.Add(claimType, values);
            }
            return values;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Properties;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The user collection actor class
    /// </summary>
    public class UserClaimsCollectionActor : BaseActor<UserClaimsCollectionState>, IUserClaimsCollectionActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimsCollectionActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserClaimsCollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Create a new user claim
        /// </summary>
        /// <param name="userClaim">The user claim</param>
        /// <exception cref="ArgumentNullException">user claim is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">User identifier not defined</exception>
        /// <exception cref="ArgumentOutOfRangeException">Claim type not defined</exception>
        public async Task Create(UserClaim userClaim)
        {
            if (userClaim == null)
            {
                throw new ArgumentNullException(nameof(userClaim));
            }
            if (userClaim.UserId == default)
            {
                throw new ArgumentOutOfRangeException(Resources.UserIdNotDefined);
            }
            if (userClaim.ClaimType == default)
            {
                throw new ArgumentOutOfRangeException(Resources.ClaimTypeNotDefined);
            }
            if (!State.UserIds.Contains(userClaim.UserId))
            {
                State.UserIds.Add(userClaim.UserId);
            }
            if (!State.ClaimTypes.ContainsKey(userClaim.ClaimType))
            {
                State.ClaimTypes.Add(userClaim.ClaimType, new HashSet<Guid>() { userClaim.UserId });
            }
            await IdentityActors.UserClaims(userClaim.UserId).Add(userClaim.ClaimType, userClaim.ClaimValue);
            await SetState();
        }

        /// <summary>
        /// Delete a new user claim
        /// </summary>
        /// <param name="userClaim">The user claim</param>
        /// <exception cref="ArgumentNullException">user claim is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">User identifier not defined</exception>
        /// <exception cref="ArgumentOutOfRangeException">Claim type not defined</exception>
        public async Task Delete(UserClaim userClaim)
        {
            if (userClaim == null)
            {
                throw new ArgumentNullException(nameof(userClaim));
            }
            if (userClaim.UserId == default)
            {
                throw new ArgumentOutOfRangeException(Resources.UserIdNotDefined);
            }
            if (userClaim.ClaimType == default)
            {
                throw new ArgumentOutOfRangeException(Resources.ClaimTypeNotDefined);
            }
            if (!State.UserIds.Contains(userClaim.UserId))
            {
                State.UserIds.Add(userClaim.UserId);
            }
            if (!State.ClaimTypes.ContainsKey(userClaim.ClaimType))
            {
                State.ClaimTypes.Add(userClaim.ClaimType, new HashSet<Guid>() { userClaim.UserId });
            }
            await IdentityActors.UserClaims(userClaim.UserId).Add(userClaim.ClaimType, userClaim.ClaimValue);
            await SetState();
        }

        /// <summary>
        /// Gets the users having a claim with the given type and value.
        /// </summary>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Claim type</exception>
        public async Task<IList<User>> GetUsers(string claimType, string claimValue)
        {
            if (string.IsNullOrWhiteSpace(claimType))
            {
                throw new ArgumentNullException(claimType);
            }
            var list = new List<Task<User?>>();
            foreach (Guid userId in State.ClaimTypes.Where(p => p.Key == claimType).SelectMany(p => p.Value))
            {
                list.Add(FindClaimUser(userId, claimType, claimValue));
            }
            return (IList<User>)(await Task.WhenAll(list)).Where(p => p != null).ToList();
        }

        private static async Task<User?> FindClaimUser(Guid userId, string claimType, string claimValue)
        {
            if (await IdentityActors.UserClaims(userId).Exist(claimType, claimValue))
            {
                return await IdentityActors.User(userId).GetUser();
            }
            return null;
        }
    }
}
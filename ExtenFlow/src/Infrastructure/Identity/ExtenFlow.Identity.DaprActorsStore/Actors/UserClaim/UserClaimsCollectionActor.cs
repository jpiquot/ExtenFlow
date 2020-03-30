using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.DaprActorsStore
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

        private static IUserClaimsActor GetUserClaimsActor(Guid userId) => ActorProxy.Create<IUserClaimsActor>(new ActorId(userId.ToString()), nameof(UserClaimsActor));

        public async Task Create(UserClaim userClaim)
        {
            if (userClaim == null)
            {
                throw new ArgumentNullException(nameof(userClaim));
            }
            if (userClaim.UserId == default)
            {
                throw new ArgumentOutOfRangeException(Resource.UserIdNotDefined);
            }
            if (userClaim.ClaimType == default)
            {
                throw new ArgumentOutOfRangeException(Resource.ClaimTypeNotDefined);
            }
            if (!State.UserIds.Contains(userClaim.UserId))
            {
                State.UserIds.Add(userClaim.UserId);
            }
            if (!State.ClaimTypes.ContainsKey(userClaim.ClaimType))
            {
                State.ClaimTypes.Add(userClaim.ClaimType, new HashSet<Guid>() { userClaim.UserId });
            }
            await GetUserClaimsActor(userClaim.UserId).Add(userClaim.ClaimType, userClaim.ClaimValue);
        }

        public Task Update(UserClaim userClaim) => throw new NotImplementedException();

        public Task<bool> Exist(UserClaim userClaim) => throw new NotImplementedException();

        public Task Delete(UserClaim userClaim) => throw new NotImplementedException();

        public Task<IList<User>> GetUsers(string type, string value) => throw new NotImplementedException();
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user roles class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserRoleCollectionActor"/>
    public class UserRoleCollectionActor : BaseActor<Dictionary<Guid, HashSet<Guid>>>, IUserRoleCollectionActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleCollectionActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserRoleCollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        private HashSet<Guid> GetUserRoles(Guid userId)
        {
            if (!State.TryGetValue(userId, out HashSet<Guid>? roles))
            {
                roles = new HashSet<Guid>();
                State.Add(userId, roles);
            }
            return roles;
        }

        /// <summary>
        /// Creates the specified user/role identifiers association.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Task Create(Guid userId, Guid roleId)
        {
            GetUserRoles(userId).Add(roleId);
            return SetState();
        }

        /// <summary>
        /// Deletes the specified user/role identifiers association.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Task Delete(Guid userId, Guid roleId)
        {
            GetUserRoles(userId).Remove(roleId);
            return SetState();
        }
    }
}
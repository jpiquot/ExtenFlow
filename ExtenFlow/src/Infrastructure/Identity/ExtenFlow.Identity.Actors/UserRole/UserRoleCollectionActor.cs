using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;
using ExtenFlow.Actors;
using ExtenFlow.Identity.Properties;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The user roles class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserRoleCollectionActor"/>
    public class UserRoleCollectionActor : ActorBase<Dictionary<Guid, HashSet<Guid>>>, IUserRoleCollectionActor
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

        /// <summary>
        /// Creates the specified user/role identifiers association.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Task Create(Guid userId, Guid roleId)
        {
            if (userId == default)
            {
                return Task.FromException<bool>(new ArgumentOutOfRangeException(Resources.UserIdNotDefined));
            }
            if (roleId == default)
            {
                return Task.FromException<bool>(new ArgumentOutOfRangeException(Resources.RoleIdNotDefined));
            }
            GetUserRoles(userId).Add(roleId);
            return SetStateData();
        }

        /// <summary>
        /// Deletes the specified user/role identifiers association.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Task Delete(Guid userId, Guid roleId)
        {
            if (userId == default)
            {
                return Task.FromException<bool>(new ArgumentOutOfRangeException(Resources.UserIdNotDefined));
            }
            if (roleId == default)
            {
                return Task.FromException<bool>(new ArgumentOutOfRangeException(Resources.RoleIdNotDefined));
            }
            GetUserRoles(userId).Remove(roleId);
            return SetStateData();
        }

        /// <summary>
        /// Exists the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>True if found, else false.</returns>
        public Task<bool> Exist(Guid userId, Guid roleId)
        {
            if (userId == default)
            {
                return Task.FromException<bool>(new ArgumentOutOfRangeException(Resources.UserIdNotDefined));
            }
            if (roleId == default)
            {
                return Task.FromException<bool>(new ArgumentOutOfRangeException(Resources.RoleIdNotDefined));
            }
            return Task.FromResult(GetUserRoles(userId).Any(p => p == roleId));
        }

        /// <summary>
        /// Gets the user role ids.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The ids of the roles of the user</returns>
        public Task<IList<Guid>> GetRoleIds(Guid userId)
        {
            if (userId == default)
            {
                return Task.FromException<IList<Guid>>(new ArgumentOutOfRangeException(Resources.UserIdNotDefined));
            }
            return Task.FromResult<IList<Guid>>(GetUserRoles(userId).ToList());
        }

        /// <summary>
        /// Gets the role user ids.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The ids of the users in the role</returns>
        public Task<IList<Guid>> GetUserIds(Guid roleId)
        {
            if (roleId == default)
            {
                return Task.FromException<IList<Guid>>(new ArgumentOutOfRangeException(Resources.RoleIdNotDefined));
            }
            return Task.FromResult<IList<Guid>>(State.Where(p => p.Value.Contains(roleId)).Select(p => p.Key).ToList());
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
    }
}
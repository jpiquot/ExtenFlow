using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// User roles collection actor
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserRoleCollectionActor : IActor
    {
        /// <summary>
        /// Creates the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        Task Create(Guid userId, Guid roleId);

        /// <summary>
        /// Deletes the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        Task Delete(Guid userId, Guid roleId);

        /// <summary>
        /// Exists the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>True if found, else false.</returns>
        Task<bool> Exist(Guid userId, Guid roleId);

        /// <summary>
        /// Gets the role ids.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The role id list</returns>
        Task<IList<Guid>> GetRoleIds(Guid userId);

        /// <summary>
        /// Gets the user ids.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The user id list</returns>
        Task<IList<Guid>> GetUserIds(Guid roleId);
    }
}
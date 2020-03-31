using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The role collection actor interface
    /// </summary>
    public interface IRoleCollectionActor : IActor
    {
        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="role">The new role properties</param>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Create(Role role);

        /// <summary>
        /// Delete the role
        /// </summary>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Delete(Guid roleId, string concurrencyString);

        /// <summary>
        /// Checks if a role with the given identifier exists.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>true if the role exists, else false.</returns>
        Task<bool> Exist(Guid roleId);

        /// <summary>
        /// Finds the name of the by normalized.
        /// </summary>
        /// <param name="normalizedRoleName">Name of the normalized role.</param>
        /// <returns>The role id</returns>
        Task<Guid?> FindByNormalizedName(string normalizedRoleName);

        /// <summary>
        /// Gets all the role ids.
        /// </summary>
        /// <returns>The role ids</returns>
        Task<IList<Guid>> GetIds();

        /// <summary>
        /// Update a existing role
        /// </summary>
        /// <param name="role">The new role properties</param>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Update(Role role);
    }
}
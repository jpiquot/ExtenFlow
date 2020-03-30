using System;
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
        /// Update a existing role
        /// </summary>
        /// <param name="role">The new role properties</param>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Update(Role role);

        /// <summary>
        /// Checks if a role with the given identifier exists.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>true if the role exists, else false.</returns>
        Task<bool> Exist(Guid roleId);

        /// <summary>
        /// Delete the role
        /// </summary>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Delete(Guid roleId, string concurrencyString);

        /// <summary>
        /// Finds the name of the by normalized.
        /// </summary>
        /// <param name="normalizedRoleName">Name of the normalized role.</param>
        /// <returns>The role id</returns>
        Task<Guid?> FindByNormalizedName(string normalizedRoleName);
    }
}
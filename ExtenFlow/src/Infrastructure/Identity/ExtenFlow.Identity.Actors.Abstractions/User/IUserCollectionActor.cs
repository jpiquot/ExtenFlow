using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The user collection actor interface
    /// </summary>
    public interface IUserCollectionActor : IActor
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Create(User user);

        /// <summary>
        /// Update a existing user
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Update(User user);

        /// <summary>
        /// Checks if a user with the given identifier exists.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if the user exists, else false.</returns>
        Task<bool> Exist(Guid userId);

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Delete(Guid userId, string concurrencyString);

        /// <summary>
        /// Finds the name of the by normalized.
        /// </summary>
        /// <param name="normalizedUserName">Name of the normalized user.</param>
        /// <returns>The user id if found, else null.</returns>
        Task<Guid?> FindByNormalizedName(string normalizedUserName);

        /// <summary>
        /// Gets all the user ids.
        /// </summary>
        /// <returns>The user ids</returns>
        Task<IList<Guid>> GetIds();
    }
}
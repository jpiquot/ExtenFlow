using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
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
        /// Checks if a user with the given identifier exists.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if the user exists, else false.</returns>
        Task<bool> Exist(string userId);

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Delete(string userId, string concurrencyString);
    }
}
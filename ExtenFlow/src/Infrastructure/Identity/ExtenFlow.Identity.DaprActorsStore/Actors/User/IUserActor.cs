using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user actor interface
    /// </summary>
    public interface IUserActor : IActor
    {
        /// <summary>
        /// Get the user name
        /// </summary>
        /// <returns>The user name</returns>
        Task<string> GetUserName();

        /// <summary>
        /// Set the user name
        /// </summary>
        /// <param name="userName">The new user name</param>
        Task SetUserName(string userName);

        /// <summary>
        /// Update user properties
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Update(User user);

        /// <summary>
        /// Deletes the specified concurrency stamp.
        /// </summary>
        /// <param name="concurrencyStamp">The concurrency stamp.</param>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Clear(string concurrencyStamp);

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns>The user object</returns>
        Task<User> GetUser();
    }
}
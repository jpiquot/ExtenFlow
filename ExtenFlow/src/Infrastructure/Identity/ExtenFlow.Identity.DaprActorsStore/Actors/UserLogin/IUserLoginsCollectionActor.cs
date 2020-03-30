using System;
using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user collection actor interface
    /// </summary>
    public interface IUserLoginsCollectionActor : IActor
    {
        /// <summary>
        /// Create a new user login
        /// </summary>
        /// <param name="user">The new user properties</param>
        Task Create(UserLogin user);

        /// <summary>
        /// Delete the user login
        /// </summary>
        Task Delete(Guid userId, string loginProvider, string providerKey);

        /// <summary>
        /// Finds the user login by provider.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>The user login object if found, else null.</returns>
        Task<UserLogin?> FindByProvider(string loginProvider, string providerKey);
    }
}
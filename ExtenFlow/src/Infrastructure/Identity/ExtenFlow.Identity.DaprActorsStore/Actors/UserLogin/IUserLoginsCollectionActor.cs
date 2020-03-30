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

        Task<UserLogin> FindByUserLogin(Guid userId, string loginProvider);

        Task<UserLogin> FindByProvider(string loginProvider, string providerKey);
    }
}
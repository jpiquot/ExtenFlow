using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using ExtenFlow.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user login actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserLoginsActor : IActor
    {
        /// <summary>
        /// Gets the user login.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>The UserLoginInfo object.</returns>
        Task<UserLoginInfo> Get(string providerName, string providerKey);

        /// <summary>
        /// Finds the user login.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>The UserLoginInfo object if exists, else null.</returns>
        Task<UserLoginInfo?> Find(string providerName, string providerKey);

        Task<IList<UserLoginInfo>> GetAll();

        Task Add(UserLoginInfo userLoginInfo);

        Task Delete(string loginProvider, string providerKey);
    }
}
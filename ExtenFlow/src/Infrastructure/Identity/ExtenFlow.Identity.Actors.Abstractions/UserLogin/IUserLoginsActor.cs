using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The user login actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserLoginsActor : IActor
    {
        /// <summary>
        /// Adds the login.
        /// </summary>
        /// <param name="userLoginInfo">The user login information.</param>
        Task AddLogin(UserLoginInfo userLoginInfo);

        /// <summary>
        /// Deletes the login.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="providerKey">The provider key.</param>
        Task DeleteLogin(string loginProvider, string providerKey);

        /// <summary>
        /// Finds the user login.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>The UserLoginInfo object if exists, else null.</returns>
        Task<UserLoginInfo?> FindLogin(string providerName, string providerKey);

        /// <summary>
        /// Gets all user logins.
        /// </summary>
        /// <returns>User login list</returns>
        Task<IList<UserLoginInfo>> GetAll();

        /// <summary>
        /// Gets the user login.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>The UserLoginInfo object.</returns>
        Task<UserLoginInfo> GetLogin(string providerName, string providerKey);
    }
}
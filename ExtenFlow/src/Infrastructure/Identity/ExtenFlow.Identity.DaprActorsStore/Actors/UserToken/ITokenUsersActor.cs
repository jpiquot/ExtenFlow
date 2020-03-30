using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The token users actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface ITokenUsersActor : IActor
    {
        /// <summary>
        /// Determines whether the token has the specified user.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name</param>
        /// <param name="tokenValue">The token value</param>
        /// <returns>True if the token has the user</returns>
        /// <exception cref="ArgumentNullException">user normalized name</exception>
        Task<bool> HasUser(string userNormalizedName, string tokenValue);

        /// <summary>
        /// Adds the token's users.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name</param>
        /// <param name="tokenValue">The token value</param>
        /// <exception cref="ArgumentNullException">user normalized name</exception>
        Task AddUser(string userNormalizedName, string tokenValue);

        /// <summary>
        /// Removes the tokens.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name</param>
        /// <param name="tokenValue">The token value</param>
        /// <exception cref="ArgumentNullException">user normalized name</exception>
        Task RemoveUser(string userNormalizedName, string tokenValue);

        /// <summary>
        /// Gets the token values for a user.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name.</param>
        /// <returns></returns>
        Task<IList<string>> GetTokenValues(string userNormalizedName);
    }
}
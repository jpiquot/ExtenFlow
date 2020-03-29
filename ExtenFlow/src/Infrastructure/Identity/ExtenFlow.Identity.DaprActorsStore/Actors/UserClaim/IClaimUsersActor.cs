using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The claim users actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IClaimUsersActor : IActor
    {
        /// <summary>
        /// Determines whether the claim has the specified user.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name</param>
        /// <param name="claimValue">The claim value</param>
        /// <returns>True if the claim has the user</returns>
        /// <exception cref="ArgumentNullException">user normalized name</exception>
        Task<bool> HasUser(string userNormalizedName, string claimValue);

        /// <summary>
        /// Adds the claim's users.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name</param>
        /// <param name="claimValue">The claim value</param>
        /// <exception cref="ArgumentNullException">user normalized name</exception>
        Task AddUser(string userNormalizedName, string claimValue);

        /// <summary>
        /// Removes the claims.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name</param>
        /// <param name="claimValue">The claim value</param>
        /// <exception cref="ArgumentNullException">user normalized name</exception>
        Task RemoveUser(string userNormalizedName, string claimValue);

        /// <summary>
        /// Gets the claim values for a user.
        /// </summary>
        /// <param name="userNormalizedName">The user normalized name.</param>
        /// <returns></returns>
        Task<IList<string>> GetClaimValues(string userNormalizedName);
    }
}
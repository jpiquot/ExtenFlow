using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user roles actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserRolesActor : IActor
    {
        /// <summary>
        /// Determines whether the user has the specified role.
        /// </summary>
        /// <param name="roleNormalizedName">Normalized name of the role.</param>
        /// <returns>True if the user has the role</returns>
        /// <exception cref="ArgumentNullException">roleNormalizedName</exception>
        Task<bool> HasRole(string roleNormalizedName);

        /// <summary>
        /// Adds the user's role.
        /// </summary>
        /// <param name="roleNormalizedName">Name of the role normalized.</param>
        /// <exception cref="ArgumentNullException">roleNormalizedName</exception>
        Task AddRole(string roleNormalizedName);

        /// <summary>
        /// Removes the role.
        /// </summary>
        /// <param name="roleNormalizedName">Name of the role normalized.</param>
        /// <exception cref="ArgumentNullException">roleNormalizedName</exception>
        Task RemoveRole(string roleNormalizedName);

        /// <summary>
        /// Gets the all the user's roles.
        /// </summary>
        /// <returns>A list of all roles</returns>
        Task<IList<string>> GetRoles();
    }
}
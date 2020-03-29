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
        /// <param name="roleId">The role identifier.</param>
        /// <returns>True if the user has the role</returns>
        /// <exception cref="ArgumentNullException">roleNormalizedName</exception>
        Task<bool> HasRole(Guid roleId);

        /// <summary>
        /// Adds the user's role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <exception cref="ArgumentNullException">roleNormalizedName</exception>
        Task AddRole(Guid roleId);

        /// <summary>
        /// Removes the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <exception cref="ArgumentNullException">roleNormalizedName</exception>
        Task RemoveRole(Guid roleId);

        /// <summary>
        /// Gets the all the user's roles.
        /// </summary>
        /// <returns>A list of all roles</returns>
        Task<IList<Guid>> GetRoles();
    }
}
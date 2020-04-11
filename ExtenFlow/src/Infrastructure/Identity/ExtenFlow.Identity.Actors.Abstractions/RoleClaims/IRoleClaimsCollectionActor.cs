using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The role collection actor interface
    /// </summary>
    public interface IRoleClaimsCollectionActor : IActor
    {
        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="roleClaim">The role claim</param>
        Task Create(RoleClaim roleClaim);

        /// <summary>
        /// Delete the role
        /// </summary>
        /// <param name="roleClaim">The role claim</param>
        Task Delete(RoleClaim roleClaim);

        /// <summary>
        /// Gets all the roles having the given claim.
        /// </summary>
        /// <param name="type">The claim type.</param>
        /// <param name="value">The claim value.</param>
        /// <returns></returns>
        Task<IList<Role>> GetRoles(string type, string value);
    }
}
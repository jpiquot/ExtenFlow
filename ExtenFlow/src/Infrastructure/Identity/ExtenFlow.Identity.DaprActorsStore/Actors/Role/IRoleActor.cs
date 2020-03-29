using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The role actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IRoleActor : IActor
    {
        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <returns>The role object</returns>
        Task<Role> GetRole();

        /// <summary>
        /// Updates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>The identity result object</returns>
        Task<IdentityResult> Update(Role role);

        /// <summary>
        /// Clears the specified concurrency string.
        /// </summary>
        /// <param name="concurrencyString">The concurrency string.</param>
        /// <returns>The identity result object</returns>
        Task<IdentityResult> Clear(string concurrencyString);
    }
}
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
        Task<Role> Get();

        /// <summary>
        /// Set the specified role value.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>The identity result object</returns>
        Task<IdentityResult> Set(Role role);

        /// <summary>
        /// Clears the specified concurrency string.
        /// </summary>
        /// <param name="concurrencyStamp">The concurrency stamp.</param>
        /// <returns>The identity result object</returns>
        Task<IdentityResult> Clear(string concurrencyStamp);
    }
}
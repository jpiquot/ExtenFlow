using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user claim actor interface
    /// </summary>
    public interface IUserClaimsActor : IActor
    {
        /// <summary>
        /// Gets the claims.
        /// </summary>
        /// <returns>The users claims</returns>
        Task<IList<Claim>> GetClaims();

        /// <summary>
        /// Adds the claims.
        /// </summary>
        /// <param name="claims">The claims.</param>
        Task AddClaims(IEnumerable<Claim> claims);

        /// <summary>
        /// Replaces the claim.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="newClaim">The new claim.</param>
        Task ReplaceClaim(string type, Claim newClaim);

        /// <summary>
        /// Removes the claims.
        /// </summary>
        /// <param name="claimTypes">The claim types</param>
        Task RemoveClaims(string[] claimTypes);
    }
}
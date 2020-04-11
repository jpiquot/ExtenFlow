using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The role claims actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IRoleClaimsActor : IActor
    {
        /// <summary>
        /// Adds the role's claims.
        /// </summary>
        /// <param name="claimType">The type of the claim</param>
        /// <param name="claimValue">The value of the claim</param>
        /// <exception cref="ArgumentNullException">claimType</exception>
        Task Add(string claimType, string claimValue);

        /// <summary>
        /// Determines whether the role has the specified claim.
        /// </summary>
        /// <param name="claimType">The type of the claim</param>
        /// <param name="claimValue">The value of the claim</param>
        /// <returns>True if the role has the claims</returns>
        /// <exception cref="ArgumentNullException">claimType</exception>
        Task<bool> Exist(string claimType, string claimValue);

        /// <summary>
        /// Gets the all the role's claimss.
        /// </summary>
        /// <returns>A list of all claims as tuples of strings (Claim Type, Claim Value)</returns>
        Task<IList<Tuple<string, string>>> GetAll();

        /// <summary>
        /// Removes the claims.
        /// </summary>
        /// <param name="claimType">The type of the claim</param>
        /// <param name="claimValue">The value of the claim</param>
        /// <exception cref="ArgumentNullException">claimType</exception>
        Task Remove(string claimType, string claimValue);
    }
}
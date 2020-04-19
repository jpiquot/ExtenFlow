using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Users.Actors
{
    /// <summary>
    /// The user claims actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserClaimsActor : IActor, IDispatchActor
    {
        /// <summary>
        /// Adds the user's claims.
        /// </summary>
        /// <param name="claimType">The type of the claim</param>
        /// <param name="claimValue">The value of the claim</param>
        /// <exception cref="ArgumentNullException">claimType</exception>
        Task Add(string claimType, string claimValue);

        /// <summary>
        /// Determines whether the user has the specified claim.
        /// </summary>
        /// <param name="claimType">The type of the claim</param>
        /// <param name="claimValue">The value of the claim</param>
        /// <returns>True if the user has the claims</returns>
        /// <exception cref="ArgumentNullException">claimType</exception>
        Task<bool> Exist(string claimType, string claimValue);

        /// <summary>
        /// Gets the all the user's claimss.
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
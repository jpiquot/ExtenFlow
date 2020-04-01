using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The user collection actor interface
    /// </summary>
    public interface IUserClaimsCollectionActor : IActor
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userClaim">The user claim</param>
        Task Create(UserClaim userClaim);

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <param name="userClaim">The user claim</param>
        Task Delete(UserClaim userClaim);

        /// <summary>
        /// Gets all the users having the given claim.
        /// </summary>
        /// <param name="type">The claim type.</param>
        /// <param name="value">The claim value.</param>
        /// <returns></returns>
        Task<IList<User>> GetUsers(string type, string value);
    }
}
using System.Threading.Tasks;

using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Queries;

namespace ExtenFlow.Identity.Services
{
    /// <summary>
    /// The user query service interface
    /// </summary>
    public interface IUserQueryService
    {
        /// <summary>
        /// Authenticates the user credentials.
        /// </summary>
        /// <returns>A <see cref="User"/> that represents an authenticated user.</returns>
        Task<User> Invoke(GetAuthenticatedUser query);

        /// <summary>
        /// Gets the user with a specified username.
        /// </summary>
        /// <param name="query">Get user query</param>
        /// <returns>The <see cref="User"/> represents the retrieved user.</returns>
        Task<User> Invoke(GetUser query);
    }
}
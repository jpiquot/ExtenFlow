using System.Security.Claims;
using System.Threading.Tasks;

using ExtenFlow.Security.Users.Queries;

namespace ExtenFlow.Security.Users.Services
{
    public interface IUserQueryService
    {
        /// <summary>
        /// Authenticates the user credentials.
        /// </summary>
        /// <returns>A <see cref="IUser"/> that represents an authenticated user.</returns>
        Task<IUser> Invoke(GetAuthenticatedUser query);

        /// <summary> Gets the authenticated user from a given <see cref="ClaimsPrincipal"/>.
        /// <returns>A <see cref="IUser"/> represents the authenticated user.</returns>
        Task<IUser> Invoke(ClaimsPrincipal principal);

        /// <summary>
        /// Gets the user with a specified username.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <returns>The <see cref="IUser"/> represents the retrieved user.</returns>
        Task<IUser> GetUserAsync(string userName);

        /// <summary>
        /// Gets the user with a specified ID.
        /// </summary>
        /// <param name="userIdentifier">The user ID.</param>
        /// <returns>A <see cref="IUser"/> represents a retrieved user.</returns>
        Task<IUser> GetUserByUniqueIdAsync(string userIdentifier);

        /// <summary>
        /// Get a forgotten password for a specified user ID.
        /// </summary>
        /// <param name="userIdentifier">The user ID.</param>
        /// <returns>A <see cref="IUser"/> represents a user with forgotton password.</returns>
        Task<IUser> GetForgotPasswordUserAsync(string userIdentifier);
    }
}
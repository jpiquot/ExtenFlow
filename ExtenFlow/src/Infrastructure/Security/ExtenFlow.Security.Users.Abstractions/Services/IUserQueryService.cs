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

        /// <summary>
        /// Gets the user with a specified username.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <returns>The <see cref="IUser"/> represents the retrieved user.</returns>
        Task<IUser> Invoke(GetUser user);
    }
}
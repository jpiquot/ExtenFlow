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
    }
}
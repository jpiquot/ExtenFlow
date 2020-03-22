using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExtenFlow.Security.Users.Services
{
    public interface IUserCommandService
    {
        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="user">The <see cref="IUser"/>.</param>
        /// <param name="password">The user password.</param>
        /// <param name="reportError">
        /// The error reported in case failure happened during the creation process.
        /// </param>
        /// <returns>A <see cref="IUser"/> represents a created user.</returns>
        Task<IUser> CreateUserAsync(IUser user, string password, Action<string, string> reportError);

        /// <summary>
        /// Change a user email.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="newEmail">The new email</param>
        /// <param name="reportError">
        /// The error reported in case failure happened during the creation process.
        /// </param>
        /// <returns>Returns <c>true</c> if the email has been changed, otherwise <c>false</c>.</returns>
        Task<bool> ChangeEmailAsync(IUser user, string newEmail, Action<string, string> reportError);

        /// <summary>
        /// Change a user password.
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="currentPassword">The current password</param>
        /// <param name="newPassword">The new password</param>
        /// <param name="reportError"></param>
        /// <returns>Returns <c>true</c> if the password has been changed, otherwise <c>false</c>.</returns>
        Task<bool> ChangePasswordAsync(IUser user, string currentPassword, string newPassword, Action<string, string> reportError);

        /// <summary>
        /// Resets the user password.
        /// </summary>
        /// <param name="userIdentifier">The user ID.</param>
        /// <param name="resetToken">The token used to reset the password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="reportError">
        /// The error reported in case failure happened during the reset process.
        /// </param>
        /// <returns>Returns <c>true</c> if the password reset, otherwise <c>false</c>.</returns>
        Task<bool> ResetPasswordAsync(string userIdentifier, string resetToken, string newPassword, Action<string, string> reportError);

        /// <summary>
        /// Creates a <see cref="ClaimsPrincipal"/> for a given user.
        /// </summary>
        /// <param name="user">The <see cref="IUser"/>.</param>
        /// <returns>The <see cref="ClaimsPrincipal"/>.</returns>
        Task<ClaimsPrincipal> CreatePrincipalAsync(IUser user);
    }
}
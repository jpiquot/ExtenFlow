using System.Threading.Tasks;

using ExtenFlow.Security.Users.Commands;

namespace ExtenFlow.Security.Users.Services
{
    /// <summary>
    /// The user command service interface
    /// </summary>
    public interface IUserCommandService
    {
        /// <summary>
        /// Registers a new user with a specified username and password.
        /// </summary>
        /// <param name="command">The register new user command</param>
        Task Invoke(RegisterNewUser command);
    }
}
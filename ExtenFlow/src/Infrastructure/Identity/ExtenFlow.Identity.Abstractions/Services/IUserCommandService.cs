using System.Threading.Tasks;

using ExtenFlow.Identity.Users.Commands;

namespace ExtenFlow.Identity.Services
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
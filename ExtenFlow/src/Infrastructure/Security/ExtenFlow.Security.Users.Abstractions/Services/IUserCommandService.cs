using System.Threading.Tasks;

using ExtenFlow.Security.Users.Commands;

namespace ExtenFlow.Security.Users.Services
{
    public interface IUserCommandService
    {
        /// <summary>
        /// Registers a new user with a specified username and password.
        /// </summary>
        Task Invoke(RegisterNewUser user);
    }
}
using System.Threading.Tasks;

using ExtenFlow.Domain;

namespace ExtenFlow.Identity.Roles.Application
{
    /// <summary>
    /// Interface IRoleService
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Submits the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task Submit(IMessage message);
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Services
{
    /// <summary>
    /// The interface for the command service
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// Submit a command
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        Task Invoke(ICommand command, CancellationToken cancellationToken = default);
    }
}
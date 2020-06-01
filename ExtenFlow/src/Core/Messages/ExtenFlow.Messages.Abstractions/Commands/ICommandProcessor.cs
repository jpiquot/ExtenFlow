using System.Threading;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Commands
{
    /// <summary>
    /// Defines an command processor
    /// </summary>
    public interface ICommandProcessor
    {
        /// <summary>
        /// Send a command. The command execution is asynchronous.
        /// </summary>
        /// <param name="command">Command to be sent.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task Send(ICommand command, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Submit a command. The command execution is synchronous.
        /// </summary>
        /// <param name="command">Command to be executed by the application.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task Submit(ICommand command, CancellationToken? cancellationToken = default);
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Commands
{
    /// <summary>
    /// Command sender
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Sends a command to a unique command handler.
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        /// <param name="command">Command object to be sent</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Task representing sending</returns>
        Task Send<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand;
    }
}
using System.Threading.Tasks;

namespace ExtenFlow.Domain
{
    /// <summary>
    /// Command dispather interface
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Submits the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task Submit(ICommand command);
    }
}
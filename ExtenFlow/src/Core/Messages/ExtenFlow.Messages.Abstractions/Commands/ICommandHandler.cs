using System.Threading.Tasks;

namespace ExtenFlow.Messages.Commands
{
    /// <summary>
    /// Interface ICommandHandler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandHandler<T>
        where T : class, ICommand
    {
        /// <summary>
        /// Tells the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        Task Handle(T command);
    }
}
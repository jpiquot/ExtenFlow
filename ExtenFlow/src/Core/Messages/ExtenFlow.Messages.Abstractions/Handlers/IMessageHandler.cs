using System.Threading.Tasks;

namespace ExtenFlow.Messages.Handlers
{
    /// <summary>
    /// Interface IMessageHandler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageHandler<T> where T : class, IMessage
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task Handle(T message);
    }
}
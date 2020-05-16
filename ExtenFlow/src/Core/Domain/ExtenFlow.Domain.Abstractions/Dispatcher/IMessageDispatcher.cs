using System.Threading.Tasks;

namespace ExtenFlow.Domain.Dispatcher
{
    /// <summary>
    /// Message dispatcher interface
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// Submit the specified envelope and get response.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns></returns>
        Task<object> Ask(Envelope envelope);

        /// <summary>
        /// Sends the specified envelope in fire and forget mode.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns></returns>
        Task Send(Envelope envelope);

        /// <summary>
        /// Submits the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns></returns>
        Task Submit(Envelope envelope);
    }
}
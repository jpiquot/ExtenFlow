using System.Threading.Tasks;

using ExtenFlow.Messages;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Message dispatcher Actor
    /// </summary>
    public interface IDispatchActor
    {
        /// <summary>
        /// Asks the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns></returns>
        Task<object> Ask(Envelope envelope);

        /// <summary>
        /// Tells the specified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns></returns>
        Task Tell(Envelope envelope);
    }
}
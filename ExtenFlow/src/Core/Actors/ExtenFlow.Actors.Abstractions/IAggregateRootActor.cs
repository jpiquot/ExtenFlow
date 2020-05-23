using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Domain;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Interface IAggregateRootActor Implements the <see cref="ExtenFlow.Domain.IAggregateRoot"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.IAggregateRoot"/>
    public interface IAggregateRootActor : IActor
    {
        /// <summary>
        /// Ask for a response to the query contained in the spécified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns></returns>
        Task<object> Ask(Envelope envelope);

        /// <summary>
        /// Notifies with the message contained in the spécified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns></returns>
        Task Notify(Envelope envelope);

        /// <summary>
        /// Tells to execute the command contained in the spécified envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <returns></returns>
        Task Tell(Envelope envelope);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr.Actors;
using ExtenFlow.Domain;
using ExtenFlow.Messages;

namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Interface IEventStreamActor Implements the <see cref="Dapr.Actors.IActor"/>
    /// </summary>
    /// <seealso cref="Dapr.Actors.IActor"/>
    public interface IEventStreamActor : IActor
    {
        /// <summary>
        /// Writes the events.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns>Task&lt;System.Int64&gt;.</returns>
        Task<long> AppendEvents(IList<IEvent> events);

        /// <summary>
        /// Gets the lastest version.
        /// </summary>
        /// <returns>The lastest version number of the stream.</returns>
        Task<long> GetLastestVersion();

        /// <summary>
        /// Resets the version value by reading the event stream store.
        /// </summary>
        /// <remarks>This operation can be very long. Reset only if the version number is corrupted.</remarks>
        /// <returns>The version number of the last session in the store</returns>
        Task<long> ResetVersion();
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Messages;

namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Interface IEventStoreTransactionActor Implements the <see cref="Dapr.Actors.IActor"/>
    /// </summary>
    /// <seealso cref="Dapr.Actors.IActor"/>
    public interface IEventStoreTransactionActor : IActor
    {
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns>Task&lt;IEnumerable&lt;IEvent&gt;&gt;.</returns>
        Task<IEnumerable<IEvent>> GetEvents();

        /// <summary>
        /// Determines whether this instance has events.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> HasEvents();

        /// <summary>
        /// Sets the events.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns>Task.</returns>
        Task SetEvents(IList<IEvent> events);
    }
}
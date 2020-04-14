using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ExtenFlow.Messages;

namespace ExtenFlow.EventStorage.DaprActors
{
    /// <summary>
    /// Class DaprActorEventStoreStream. Implements the <see cref="ExtenFlow.EventStorage.IEventStoreStream"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.EventStorage.IEventStoreStream"/>
    internal class DaprActorEventStoreStream : IEventStoreStream
    {
        private readonly IEventStoreStreamActor _actor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DaprActorEventStoreStream"/> class.
        /// </summary>
        /// <param name="actor">The actor.</param>
        public DaprActorEventStoreStream(IEventStoreStreamActor actor)
        {
            _actor = actor;
        }

        /// <summary>
        /// Appends the specified events to the store stream.
        /// </summary>
        /// <param name="events">The events to persist.</param>
        /// <returns>Task.</returns>
        public Task Append(IList<IEvent> events)
            => _actor.Append(events);

        public Task<IList<IEvent>> Read(Guid? afterId = null, int take = 0)
            => _actor.Read(afterId, take);
    }
}
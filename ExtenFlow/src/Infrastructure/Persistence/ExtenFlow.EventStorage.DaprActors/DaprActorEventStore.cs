using System;
using System.Threading.Tasks;

using ExtenFlow.Actors;

namespace ExtenFlow.EventStorage.DaprActors
{
    /// <summary>
    /// Class DaprActorEventStore. Implements the <see cref="ExtenFlow.EventStorage.IEventStore"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.EventStorage.IEventStore"/>
    public class DaprActorEventStore : IEventStore
    {
        private readonly Func<IActorSystem> _getActorSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="DaprActorEventStore"/> class.
        /// </summary>
        /// <param name="getActorSystem">Get the actor system.</param>
        public DaprActorEventStore(Func<IActorSystem> getActorSystem)
        {
            _getActorSystem = getActorSystem;
        }

        /// <summary>
        /// Gets the store stream.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns>Task&lt;IEventStoreStream&gt;.</returns>
        public Task<IEventStoreStream> GetStoreStream(string aggregateType, string aggregateId)
            => Task.FromResult<IEventStoreStream>(new DaprActorEventStoreStream(_getActorSystem()
                .Create<IEventStoreStreamActor, string>(EventStoreStreamActor.CreateStreamId(aggregateType, aggregateId))));
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using ExtenFlow.Actors;
using ExtenFlow.Messages;
using ExtenFlow.Messages.Events;

namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Class EventStoreStream. Implements the <see cref="IEventStore"/>
    /// </summary>
    /// <seealso cref="IEventStore"/>
    public class EventStore : IEventStore
    {
        private readonly IActorSystem _actorSystem;
        private readonly string _streamId;
        private IEventStoreActor? _storeActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStore"/> class.
        /// </summary>
        /// <param name="streamId">The stream name.</param>
        /// <param name="actorSystem"></param>
        public EventStore(string streamId, IActorSystem? actorSystem = null)
        {
            _streamId = streamId;
            _actorSystem = actorSystem ?? new ActorSystem();
        }

        private IEventStoreActor StoreActor => _storeActor ?? (_storeActor = _actorSystem.Create<IEventStoreActor>(_streamId));

        /// <summary>
        /// Appends the specified events to the store stream.
        /// </summary>
        /// <param name="events">The events to persist.</param>
        /// <returns>The stored events version.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<string> Append(IList<IEvent> events)
            => (await StoreActor.AppendEvents(events)).ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Reads the events from the store.
        /// </summary>
        /// <param name="afterVersion">
        /// Ignore all events before the event with the given identifier, including itself.
        /// </param>
        /// <param name="take">Maximum number of events to read.</param>
        /// <returns>The list of the matching events.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<(IList<IEvent>? events, string? version)> Read(string? afterVersion = null, int take = 0)
        {
            long fromVersion = 0;
            long latestVersion = await StoreActor.GetLastestVersion();
            if (!string.IsNullOrWhiteSpace(afterVersion))
            {
                if (!long.TryParse(afterVersion, out fromVersion) || fromVersion == 0 || fromVersion > latestVersion)
                {
                    // Invalid event store transaction identifier : '%1'.
                    throw new ArgumentOutOfRangeException(nameof(afterVersion), string.Format(CultureInfo.CurrentCulture, ExtenFlow.Messages.Properties.Resources.InvalidEventStoreTransactionId, afterVersion));
                }
            }
            var events = new List<IEvent>();
            long i = 0L;
            while (fromVersion + i < latestVersion)
            {
                i++;
                events.AddRange(
                    await _actorSystem
                        .Create<IEventStoreTransactionActor>(
                            EventStoreActor.GetTransactionId(_streamId, fromVersion + i)
                                )
                            .GetEvents()
                        );
                if (take != 0L && i >= take)
                {
                    break;
                }
            }
            if (i == 0L)
            {
                return (null, null);
            }
            return (events, (fromVersion + i).ToString(CultureInfo.InvariantCulture));
        }
    }
}
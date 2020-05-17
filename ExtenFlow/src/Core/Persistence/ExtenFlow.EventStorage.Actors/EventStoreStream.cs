using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using ExtenFlow.Actors;
using ExtenFlow.Domain;

namespace ExtenFlow.EventStorage.Actors
{
    /// <summary>
    /// Class EventStoreStream. Implements the <see cref="ExtenFlow.EventStorage.IEventStoreStream"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.EventStorage.IEventStoreStream"/>
    public class EventStoreStream : IEventStoreStream
    {
        private readonly IActorSystem _actorSystem;
        private readonly string _streamId;
        private IEventStreamActor? _streamActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStoreStream"/> class.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="actorSystem"></param>
        public EventStoreStream(string aggregateType, string aggregateId, IActorSystem? actorSystem = null)
        {
            _streamId = $"{aggregateType}-[{aggregateId}]";
            _actorSystem = actorSystem ?? new ActorSystem();
        }

        private IEventStreamActor StreamActor => _streamActor ?? (_streamActor = _actorSystem.Create<IEventStreamActor>(_streamId));

        /// <summary>
        /// Appends the specified events to the store stream.
        /// </summary>
        /// <param name="events">The events to persist.</param>
        /// <returns>The stored events version.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<string> Append(IList<IEvent> events)
            => (await StreamActor.AppendEvents(events)).ToString(CultureInfo.InvariantCulture);

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
            if (!string.IsNullOrWhiteSpace(afterVersion))
            {
                if (!long.TryParse(afterVersion, out fromVersion))
                {
                    // Invalid stream version '%1'. It should be a long integer value.
                    throw new ArgumentOutOfRangeException(nameof(afterVersion), string.Format(CultureInfo.CurrentCulture, Properties.Resources.InvalidStreamNumberNotLong, afterVersion));
                }
            }
            long latestVersion = await StreamActor.GetLastestVersion();
            var events = new List<IEvent>();
            long i = 0L;
            while (fromVersion + i < latestVersion)
            {
                i++;
                events.AddRange(
                    await _actorSystem
                        .Create<IEventStreamSessionActor>(
                            EventStreamSessionHelper
                                .GetSessionId(_streamId, fromVersion + i)
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
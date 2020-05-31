using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Events
{
    /// <summary>
    /// Class InMemoryEventStore.
    /// </summary>
    /// <remarks>Only used for testing.</remarks>
    public class InMemoryEventStore : IEventStore
    {
        private Dictionary<long, IList<IEvent>>? _events;

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        protected Dictionary<long, IList<IEvent>> Events => _events ?? (_events = new Dictionary<long, IList<IEvent>>());

        /// <summary>
        /// Appends the specified events to the store stream.
        /// </summary>
        /// <param name="events">The events to persist.</param>
        /// <returns>The stored events version.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<string> Append(IList<IEvent> events)
        {
            long version = Events.LastOrDefault().Key + 1;

            Events.Add(version, events);
            return Task.FromResult(version.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Reads the events from the store.
        /// </summary>
        /// <param name="afterVersion">
        /// Ignore all events before the event with the given identifier, including itself.
        /// </param>
        /// <param name="take">Maximum number of events to read.</param>
        /// <returns>The list of the matching events.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<(IList<IEvent>? events, string? version)> Read(string? afterVersion = null, int take = 0)
        {
            long fromVersion = 0;
            if (!string.IsNullOrWhiteSpace(afterVersion))
            {
                if (!long.TryParse(afterVersion, out fromVersion) || !Events.ContainsKey(fromVersion))
                {
                    // Invalid event store transaction identifier : '%1'.
                    throw new ArgumentOutOfRangeException(nameof(afterVersion), string.Format(CultureInfo.CurrentCulture, Properties.Resources.InvalidEventStoreTransactionId, afterVersion));
                }
            }
            IEnumerable<KeyValuePair<long, IList<IEvent>>> query = Events.Where(p => fromVersion == 0 || p.Key > fromVersion);
            if (take != 0)
            {
                query = query.Take(take);
            }
            var events = query.SelectMany(p => p.Value).ToList();
            long version = query.Select(p => p.Key).LastOrDefault();
            if (version == 0L)
            {
                return Task.FromResult<(IList<IEvent>?, string?)>((null, null));
            }
            return Task.FromResult(((IList<IEvent>?)events, (string?)version.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
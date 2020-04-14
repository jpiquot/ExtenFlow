using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using ExtenFlow.Messages;

namespace ExtenFlow.EventStorage.DaprActors
{
    /// <summary>
    /// Class EventStoreStream State.
    /// </summary>
    public class EventStoreStreamState
    {
        private List<IEvent>? _events;

        /// <summary>
        /// Appends the specified events.
        /// </summary>
        /// <param name="events">The events.</param>
        internal void Append(IList<IEvent> events)
        {
            if (events == null || events.Count < 1)
            {
                // List of events empty or null.
                throw new ArgumentException(Properties.Resources.NullOrEmptyEventList, nameof(events));
            }
            if (_events == null)
            {
                _events = events.ToList();
            }
            else
            {
                _events.AddRange(events);
            }
        }

        /// <summary>
        /// Reads the events from the store.
        /// </summary>
        /// <param name="afterId">
        /// Ignore all events before the event with the given identifier, including itself.
        /// </param>
        /// <param name="take">Maximum number of events to read.</param>
        /// <returns>The list of the matching events.</returns>
        /// <exception cref="System.ArgumentException">afterId</exception>
        internal IList<IEvent> Read(Guid? afterId = null, int take = 0)
        {
            if (_events == null)
            {
                return Array.Empty<IEvent>();
            }
            if (afterId == null)
            {
                if (take == 0)
                {
                    return _events;
                }
                else if (_events.Count <= take)
                {
                    return _events;
                }
            }
            var result = new List<IEvent>((take == 0) ? _events.Count : Math.Min(take, _events.Count));
            bool afterIdFound = false;
            foreach (IEvent @event in _events)
            {
                if (afterIdFound || afterId == null)
                {
                    result.Add(@event);
                }
                if (afterId != null && @event.MessageId == afterId)
                {
                    afterIdFound = true;
                }
            }
            if (afterId != null && !afterIdFound)
            {
                // The event with Id='{0}' not found in the event store.
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.EventNotFoundInStore, afterId), nameof(afterId));
            }
            return _events;
        }
    }
}
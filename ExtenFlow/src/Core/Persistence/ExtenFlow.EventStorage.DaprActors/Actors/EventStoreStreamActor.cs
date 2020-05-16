using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Domain;

namespace ExtenFlow.EventStorage.DaprActors
{
    /// <summary>
    /// Class EventStoreStreamActor. Implements the <see cref="ExtenFlow.Actors.ActorBase{EventStoreStreamState}"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Actors.ActorBase{EventStoreStreamState}"/>
    public class EventStoreStreamActor : ActorBase<EventStoreStreamState>, IEventStoreStreamActor, IEventStoreStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventStoreStreamActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="actorManager">The actor manager.</param>
        public EventStoreStreamActor(ActorService actorService, ActorId actorId, IActorStateManager? actorManager) : base(actorService, actorId, actorManager)
        {
        }

        /// <summary>
        /// Creates the stream identifier.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns>System.String.</returns>
        public static string CreateStreamId(string aggregateType, string aggregateId) => $"{aggregateType}-[{aggregateId}]";

        /// <summary>
        /// Appends the specified events.
        /// </summary>
        /// <param name="events">The events.</param>
        public async Task Append(IList<IEvent> events)
        {
            if (events == null || events.Count < 1)
            {
                // List of events empty or null.
                throw new ArgumentException(Properties.Resources.NullOrEmptyEventList, nameof(events));
            }
            State.Append(events);
            await SetStateData();
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
        public Task<IList<IEvent>> Read(Guid? afterId = null, int take = 0)
        {
            if (StateIsNull())
            {
                return Task.FromResult<IList<IEvent>>(Array.Empty<IEvent>());
            }
            return Task.FromResult(State.Read(afterId, take));
        }

        /// <summary>
        /// Creates new state.
        /// </summary>
        /// <returns>TState.</returns>
        protected override EventStoreStreamState NewState() => new EventStoreStreamState();
    }
}
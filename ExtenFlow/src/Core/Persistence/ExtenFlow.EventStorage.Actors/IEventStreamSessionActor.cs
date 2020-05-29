using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Domain;
using ExtenFlow.Messages;

namespace ExtenFlow.EventStorage.Actors
{
    internal interface IEventStreamSessionActor : IActor
    {
        Task<IList<IEvent>> GetEvents();

        Task<bool> HasEvents();

        Task SetEvents(IList<IEvent> events);
    }
}
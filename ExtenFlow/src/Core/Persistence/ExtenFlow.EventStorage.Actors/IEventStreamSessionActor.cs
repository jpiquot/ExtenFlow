using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Domain;

namespace ExtenFlow.EventStorage.Actors
{
    internal interface IEventStreamSessionActor : IActor
    {
        Task<IList<IEvent>> GetEvents();

        Task<bool> HasEvents();

        Task SetEvents(IList<IEvent> events);
    }
}
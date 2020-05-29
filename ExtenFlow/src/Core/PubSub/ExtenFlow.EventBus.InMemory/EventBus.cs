using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExtenFlow.Messages;

namespace ExtenFlow.EventBus.InMemory
{
    /// <summary>
    /// Class EventBus. Implements the <see cref="ExtenFlow.EventBus.IEventBus"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.EventBus.IEventBus"/>
    public class EventBus : IEventBus
    {
        public Task ConfirmSend(Guid batchId) => throw new NotImplementedException();

        public Task<IMessage?> ReadNext() => throw new NotImplementedException();

        public Task RemoveMessage(string id) => throw new NotImplementedException();

        public Task<Guid> Send(IList<IEvent> events) => throw new NotImplementedException();
    }
}
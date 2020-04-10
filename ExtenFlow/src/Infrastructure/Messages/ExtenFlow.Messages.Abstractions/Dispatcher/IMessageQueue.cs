using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Dispatcher
{
    /// <summary>
    /// Message receiver
    /// </summary>
    public interface IMessageQueue
    {
        /// <summary>
        /// Reads the next message.
        /// </summary>
        /// <returns>The message one is present in the queue, else null.</returns>
        Task<IMessage?> ReadNext();

        Task RemoveMessage(Guid messageId);

        Task Send(IList<IEvent> events);
    }
}
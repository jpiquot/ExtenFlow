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
        /// Confirms the event send=t in the batch.
        /// </summary>
        /// <param name="batchId">The batch identifier.</param>
        /// <returns></returns>
        Task ConfirmSend(Guid batchId);

        /// <summary>
        /// Reads the next message.
        /// </summary>
        /// <returns>The message one is present in the queue, else null.</returns>
        Task<IMessage?> ReadNext();

        /// <summary>
        /// Removes the message.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns></returns>
        Task RemoveMessage(Guid messageId);

        /// <summary>
        /// Sends the specified events.
        /// </summary>
        /// <param name="events">The events.</param>
        /// <returns>The batchId needed to confirm events.</returns>
        Task<Guid> Send(IList<IEvent> events);
    }
}
using System;

using ExtenFlow.Infrastructure;
using ExtenFlow.Messages.Brighter.Requests;

using Newtonsoft.Json;

using Paramore.Brighter;

namespace ExtenFlow.Messages.Brighter
{
    /// <summary>
    /// Class EventMapper. Implements the <see cref="Paramore.Brighter.IAmAMessageMapper{T}"/>
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <seealso cref="Paramore.Brighter.IAmAMessageMapper{T}"/>
    public class EventMapper<TEvent> : IAmAMessageMapper<BrighterEvent>
        where TEvent : class, IEvent
    {
        /// <summary>
        /// Maps a command to a brighter message.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <returns>Paramore.Brighter.Message.</returns>
        /// <exception cref="System.ArgumentNullException">domainEvent</exception>
        public Paramore.Brighter.Message MapToMessage(BrighterEvent domainEvent)
        {
            _ = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));
            var header = new MessageHeader(
                messageId: domainEvent.Id,
                topic: domainEvent.Event.AggregateType,
                messageType: MessageType.MT_EVENT,
                correlationId: domainEvent.Event.CorrelationId.ToGuidOrDefault());
            var body = new MessageBody(JsonConvert.SerializeObject(new Envelope(domainEvent.Event)));
            var message = new Paramore.Brighter.Message(header, body);
            return message;
        }

        /// <summary>
        /// Maps the brighter message to an event.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>BrighterEvent&lt;T&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">message</exception>
        public BrighterEvent MapToRequest(Paramore.Brighter.Message message)
        {
            _ = message ?? throw new ArgumentNullException(nameof(message));
            return new BrighterEvent((IEvent)JsonConvert.DeserializeObject<Envelope>(message.Body.Value).Message);
        }
    }
}
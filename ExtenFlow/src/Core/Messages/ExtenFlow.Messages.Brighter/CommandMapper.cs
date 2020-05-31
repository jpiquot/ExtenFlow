using System;

using ExtenFlow.Infrastructure;
using ExtenFlow.Messages.Brighter.Requests;

using Newtonsoft.Json;

using Paramore.Brighter;

namespace ExtenFlow.Messages.Brighter
{
    /// <summary>
    /// Class CommandMapper. Implements the <see cref="Paramore.Brighter.IAmAMessageMapper{T}"/>
    /// </summary>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    /// <seealso cref="Paramore.Brighter.IAmAMessageMapper{T}"/>
    public class CommandMapper<TCommand> :
        IAmAMessageMapper<BrighterCommand>
        where TCommand : class, ICommand
    {
        /// <summary>
        /// Maps to message.
        /// </summary>
        /// <param name="command">The command to serialize.</param>
        /// <returns>Paramore.Brighter.Message.</returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public Paramore.Brighter.Message MapToMessage(BrighterCommand command)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            var header = new MessageHeader(
                messageId: ((Paramore.Brighter.IRequest)command).Id,
                topic: command.Command.AggregateType,
                messageType: MessageType.MT_COMMAND,
                correlationId: command.Command.CorrelationId.ToGuidOrDefault());
            var body = new MessageBody(JsonConvert.SerializeObject(new Envelope(command.Command)));
            var message = new Paramore.Brighter.Message(header, body);
            return message;
        }

        /// <summary>
        /// Maps to request.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>T.</returns>
        public BrighterCommand MapToRequest(Paramore.Brighter.Message message)
        {
            _ = message ?? throw new ArgumentNullException(nameof(message));
            return new BrighterCommand((ICommand)JsonConvert.DeserializeObject<Envelope>(message.Body.Value).Message);
        }
    }
}
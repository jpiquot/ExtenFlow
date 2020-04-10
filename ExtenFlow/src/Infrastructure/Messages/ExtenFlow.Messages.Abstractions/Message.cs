using System;
using System.Diagnostics;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages
{
    /// <summary>
    /// The base class for all messages
    /// </summary>
    [DebuggerDisplay("{AggregateType}:{AggregateId}")]
    public abstract class Message : IMessage
    {
        /// <summary>
        /// The message default constructor
        /// </summary>
        /// <remarks>Do not use this constructor. For serializer usage only.</remarks>
        [Obsolete("Can only be used by serializers")]
        protected Message()
        {
            UserId = string.Empty;
            CorrelationId = Guid.NewGuid();
            DateTime = DateTimeOffset.Now;
            MessageId = Guid.NewGuid();
            AggregateType = string.Empty;
            AggregateId = null;
        }

        /// <summary>
        /// The base message constructor
        /// </summary>
        /// <param name="aggregateType">The aggregate that will handle or has handled the message</param>
        /// <param name="aggregateId">The aggregate id</param>
        /// <param name="userId">The user that created the message</param>
        /// <param name="correlationId">The correlation id to link messages</param>
        /// <param name="messageId">The id of the message</param>
        /// <param name="dateTime">The date and time the message was created</param>
        protected Message(string aggregateType, string? aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (string.IsNullOrWhiteSpace(aggregateType))
            {
                throw new ArgumentNullException(nameof(aggregateType));
            }
            if (correlationId == null || correlationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(correlationId));
            }
            if (messageId == null || messageId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(messageId));
            }
            UserId = userId;
            CorrelationId = correlationId;
            DateTime = dateTime;
            MessageId = messageId;
            AggregateId = aggregateId;
            AggregateType = aggregateType;
        }

        /// <summary>
        /// The aggregate id
        /// </summary>
        public string? AggregateId { get; [Obsolete]set; }

        /// <summary>
        /// The type of the aggragate that will handle the message. Or the aggregate that created
        /// the message.
        /// </summary>
        public string AggregateType { get; [Obsolete]set; }

        /// <summary>
        /// The correlation id that links all the related messages together. Can be assimilated to a
        /// transaction id.
        /// </summary>
        public Guid CorrelationId { get; [Obsolete]set; }

        /// <summary>
        /// Message created date and time
        /// </summary>
        public DateTimeOffset DateTime { get; [Obsolete]set; }

        /// <summary>
        /// The message unique identifier
        /// </summary>
        public Guid MessageId { get; [Obsolete]set; }

        /// <summary>
        /// The id of the message creator
        /// </summary>
        public string UserId { get; [Obsolete]set; }
    }
}
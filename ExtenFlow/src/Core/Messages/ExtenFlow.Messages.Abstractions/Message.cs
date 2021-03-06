﻿using System;
using System.Diagnostics;

using ExtenFlow.Infrastructure;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages
{
    /// <summary>
    /// The base class for all messages.
    /// </summary>
    [DebuggerDisplay("{AggregateType}:{AggregateId}")]
    public abstract class Message : IMessage
    {
        /// <summary>
        /// The message default constructor.
        /// </summary>
        /// <remarks>Do not use this constructor. For serializer usage only.</remarks>
        [Obsolete("Can only be used by serializers")]
        protected Message()
        {
            UserId = string.Empty;
            DateTime = DateTimeOffset.Now;
            Id = Guid.NewGuid().ToBase64String();
            CorrelationId = Id;
            AggregateType = string.Empty;
            AggregateId = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="aggregateType">The aggregate that will handle or has handled the message.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        /// <exception cref="ArgumentNullException">userId</exception>
        /// <exception cref="ArgumentNullException">aggregateType</exception>
        /// <exception cref="ArgumentNullException">correlationId</exception>
        /// <exception cref="ArgumentNullException">id</exception>
        protected Message(string aggregateType, string aggregateId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (string.IsNullOrWhiteSpace(aggregateType))
            {
                throw new ArgumentNullException(nameof(aggregateType));
            }
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                throw new ArgumentNullException(nameof(correlationId));
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            UserId = userId;
            DateTime = dateTime ?? DateTimeOffset.Now;
            Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToBase64String() : id;
            CorrelationId = string.IsNullOrWhiteSpace(correlationId) ? Id : correlationId;
            AggregateId = aggregateId;
            AggregateType = aggregateType;
        }

        /// <summary>
        /// The aggregate that will handle or has handled the message.
        /// </summary>
        public string AggregateId { get; [Obsolete]set; }

        /// <summary>
        /// The type of the aggragate that will handle the message. Or the aggregate that created
        /// the message.
        /// </summary>
        public string AggregateType { get; [Obsolete]set; }

        /// <summary>
        /// The correlation id that links all the related messages together. Can be assimilated to a
        /// transaction id.
        /// </summary>
        public string CorrelationId { get; [Obsolete]set; }

        /// <summary>
        /// Message created date and time.
        /// </summary>
        public DateTimeOffset DateTime { get; [Obsolete]set; }

        /// <summary>
        /// The message unique identifier.
        /// </summary>
        public string Id { get; [Obsolete]set; }

        /// <summary>
        /// The id of the message creator.
        /// </summary>
        public string UserId { get; [Obsolete]set; }
    }
}
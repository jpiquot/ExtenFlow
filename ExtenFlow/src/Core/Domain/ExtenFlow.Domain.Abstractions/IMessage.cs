﻿using System;

namespace ExtenFlow.Domain
{
    /// <summary>
    /// The base class for all messages
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// The aggregate id
        /// </summary>
        string? AggregateId { get; }

        /// <summary>
        /// The aggregate type
        /// </summary>
        string AggregateType { get; }

        /// <summary>
        /// The correlation id is used to link messages together. It can be considered as the
        /// transaction id. For example, all the events generated by a command have the same
        /// correlation Id as the original command.
        /// </summary>
        string CorrelationId { get; }

        /// <summary>
        /// The date and time of when the message was submitted
        /// </summary>
        DateTimeOffset DateTime { get; }

        /// <summary>
        /// The message unique identifier
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The user that submitted the message
        /// </summary>
        string UserId { get; }
    }
}
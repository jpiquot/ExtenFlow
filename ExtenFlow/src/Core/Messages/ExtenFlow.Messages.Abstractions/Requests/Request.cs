using System;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages
{
    /// <summary>
    /// The base class for all requests
    /// </summary>
    public abstract class Request : Message, IRequest
    {
        /// <summary>
        /// The request base class contructor
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected Request()
        {
            AggregateId = string.Empty;
            AggregateType = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Request"/> class.
        /// </summary>
        /// <param name="aggregateType">The aggregate that will handle or has handled the message.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        protected Request(string aggregateType, string? aggregateId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null) : base(aggregateType, aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
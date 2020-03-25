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
        /// The base request constructor
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate that will handle the request</param>
        /// <param name="aggregateId">The id of the aggregate</param>
        /// <param name="userId">The id of the user submitting the request</param>
        /// <param name="correlationId">The correlation id used to link requests and messages</param>
        /// <param name="messageId">The id of the request</param>
        /// <param name="dateTime">The date and time of the submission</param>
        protected Request(string aggregateType, string? aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
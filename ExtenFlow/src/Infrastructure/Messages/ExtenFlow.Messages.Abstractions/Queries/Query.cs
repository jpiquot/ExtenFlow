using System;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages
{
    /// <summary>
    /// The base class for queries
    /// </summary>
    /// <typeparam name="T">The type of the query result</typeparam>
    public abstract class Query<T> : Request, IQuery<T>
    {
        /// <summary>
        /// The query base class contructor
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected Query()
        {
        }

        /// <summary>
        /// The base query constructor
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate that will handle the query</param>
        /// <param name="aggregateId">The id of the aggregate</param>
        /// <param name="userId">The id of the user submitting the query</param>
        /// <param name="correlationId">The correlation id used to link queries and result messages</param>
        /// <param name="messageId">The id of the query</param>
        /// <param name="dateTime">The date and time of the submission</param>
        protected Query(string aggregateType, string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
using System;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Domain
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
        /// Initializes a new instance of the <see cref="Query{T}"/> class.
        /// </summary>
        /// <param name="aggregateType">The aggregate that will handle or has handled the message.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        protected Query(string aggregateType, string aggregateId, string userId, string? correlationId, string? id, DateTimeOffset? dateTime) : base(aggregateType, aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
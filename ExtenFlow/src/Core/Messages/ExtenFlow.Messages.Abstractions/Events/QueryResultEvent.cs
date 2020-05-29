using System;

namespace ExtenFlow.Messages
{
    /// <summary>
    /// Query result event
    /// </summary>
    /// <seealso cref="Event"/>
    public class QueryResultEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryResultEvent"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public QueryResultEvent()
        {
            Result = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryResultEvent"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="result">The result.</param>
        public QueryResultEvent(IQuery query, object result) : base(query?.AggregateType ?? throw new ArgumentNullException(nameof(query)), query.AggregateId, query.UserId, query.CorrelationId, null, DateTimeOffset.Now)
        {
            Result = result;
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public object Result { get; }
    }
}
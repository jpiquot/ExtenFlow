using System;

using ExtenFlow.Messages;

namespace ExtenFlow.Security.Users.Queries
{
    /// <summary>
    /// The base clas for quering the user aggregate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UserQuery<T> : Query<T>
    {
        /// <summary>
        /// The user query constructor
        /// </summary>
        /// <remarks>Do not use. Defined for serializers only.</remarks>
        [Obsolete("Can only be used by serializers", true)]
        protected UserQuery()
        {
            AggregateType = "User";
        }

        /// <summary>
        /// </summary>
        /// <param name="aggregateId">The user id (name) to query</param>
        /// <param name="userId">The user submitting the query</param>
        /// <param name="correlationId">The correlation id</param>
        /// <param name="messageId">The id of the user query</param>
        /// <param name="dateTime">The creation date and time of the user query</param>
        protected UserQuery(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base("User", aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
using System;

using ExtenFlow.Identity.Models;

using Newtonsoft.Json;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Identity.Queries
{
    /// <summary>
    /// Get user query
    /// </summary>
    public class GetUser : UserQuery<User>
    {
        /// <summary>
        /// Get user query constructor
        /// </summary>
        /// <remarks>Do not use. Defined for serializers.</remarks>
        [Obsolete("Can only be used by serializers", true)]
        protected GetUser()
        {
        }

        /// <summary>
        /// Get user query constructor
        /// </summary>
        /// <param name="aggregateId">The id (name) of the user to retreive</param>
        /// <param name="userId">The user id of the user submitting the query</param>
        public GetUser(string aggregateId, string userId) : this(aggregateId, userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Get user query constructor
        /// </summary>
        /// <param name="aggregateId">The id (name) of the user to retreive</param>
        /// <param name="userId">The user id of the user submitting the query</param>
        /// <param name="correlationId">The correlation id</param>
        public GetUser(string aggregateId, string userId, Guid correlationId) : this(aggregateId, userId, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Get user query constructor
        /// </summary>
        /// <param name="aggregateId">The id (name) of the user to retreive</param>
        /// <param name="userId">The user id of the user submitting the query</param>
        /// <param name="correlationId">The correlation id</param>
        /// <param name="messageId">The id of the query</param>
        /// <param name="dateTime">The creation date and time of the query</param>
        [JsonConstructor]
        public GetUser(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateId, userId, correlationId, messageId, dateTime)
        {
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
        }

        /// <summary>
        /// The name of the user to query
        /// </summary>
#pragma warning disable CS8603 // Possible null reference return.
        public string UserName => AggregateId;
#pragma warning restore CS8603 // Possible null reference return.
    }
}
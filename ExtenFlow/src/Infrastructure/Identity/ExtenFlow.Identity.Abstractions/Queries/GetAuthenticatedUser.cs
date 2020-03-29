using System;

using ExtenFlow.Identity.Models;

using Newtonsoft.Json;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Identity.Queries
{
    /// <summary>
    /// Authenticate and get user command
    /// </summary>
    public class GetAuthenticatedUser : UserQuery<User>
    {
        /// <summary>
        /// Authenticate and get user command constructor
        /// </summary>
        /// <remarks>Do not use. Defined for serializers.</remarks>
        [Obsolete("Can only be used by serializers", true)]
        protected GetAuthenticatedUser()
        {
            Password = string.Empty;
        }

        /// <summary>
        /// Authenticate and get user command constructor
        /// </summary>
        /// <param name="aggregateId">The id of the user to authenticate</param>
        /// <param name="password">Password for the authentication</param>
        public GetAuthenticatedUser(string aggregateId, string password) : this(aggregateId, password, aggregateId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Authenticate and get user command constructor
        /// </summary>
        /// <param name="aggregateId">The id of the user to authenticate</param>
        /// <param name="password">Password for the authentication</param>
        /// <param name="correlationId">Correlation id</param>
        public GetAuthenticatedUser(string aggregateId, string password, Guid correlationId) : this(aggregateId, password, aggregateId, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Authenticate and get user command constructor
        /// </summary>
        /// <param name="aggregateId">The id of the user to authenticate</param>
        /// <param name="password">Password for the authentication</param>
        /// <param name="userId">User that requests a authentication</param>
        /// <param name="correlationId">Correlation id</param>
        /// <param name="messageId">The id of this query</param>
        /// <param name="dateTime">The query creation date and time</param>
        [JsonConstructor]
        public GetAuthenticatedUser(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateId, userId, correlationId, messageId, dateTime)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
            Password = password;
        }

        /// <summary>
        /// Password used for authentication
        /// </summary>
        public string Password { get; [Obsolete]set; }

#pragma warning disable CS8603 // Possible null reference return.

        /// <summary>
        /// The user to authenticate
        /// </summary>
        public string UserName => AggregateId;

#pragma warning restore CS8603 // Possible null reference return.
    }
}
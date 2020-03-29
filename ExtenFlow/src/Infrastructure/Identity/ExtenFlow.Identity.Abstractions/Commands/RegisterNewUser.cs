using System;

using Newtonsoft.Json;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete
#pragma warning disable CS8603 // Possible null reference return.

namespace ExtenFlow.Identity.Users.Commands
{
    /// <summary>
    /// Register a new user command
    /// </summary>
    public class RegisterNewUser : UserCommand
    {
        /// <summary>
        /// Register new user command constructor
        /// </summary>
        /// <remarks>Do not use. Defined to be used by serializers</remarks>
        [Obsolete("Can only be used by serializers", true)]
        protected RegisterNewUser()
        {
            Password = string.Empty;
        }

        /// <summary>
        /// Register new user command constructor
        /// </summary>
        /// <param name="aggregateId">The id of the new user</param>
        /// <param name="password">The user password</param>
        public RegisterNewUser(string aggregateId, string password) : this(aggregateId, password, aggregateId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Register new user command constructor
        /// </summary>
        /// <param name="aggregateId">The id of the new user</param>
        /// <param name="password">The user password</param>
        /// <param name="correlationId">The correlation id</param>
        public RegisterNewUser(string aggregateId, string password, Guid correlationId) : this(aggregateId, password, aggregateId, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Register new user command constructor
        /// </summary>
        /// <param name="aggregateId">The id of the new user</param>
        /// <param name="password">The user password</param>
        /// <param name="userId">The id of the user submitting this command</param>
        /// <param name="correlationId">The correlation id</param>
        /// <param name="messageId">The id of this command</param>
        /// <param name="dateTime">The created date and time of the command</param>
        [JsonConstructor]
        public RegisterNewUser(string aggregateId, string password, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateId, userId, correlationId, messageId, dateTime)
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
        /// The password of the new user
        /// </summary>
        public string Password { get; [Obsolete]set; }

        /// <summary>
        /// The new user name
        /// </summary>
        public string UserName => AggregateId;
    }
}
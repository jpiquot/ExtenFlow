using System;

using ExtenFlow.Messages;

using Newtonsoft.Json;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Security.Users.Abstractions.Queries
{
    public class GetAuthenticatedUser : Query<IUser>
    {
        [Obsolete("Can only be used by serializers", true)]
        protected GetAuthenticatedUser()
        {
            Password = string.Empty;
        }

        public GetAuthenticatedUser(string aggregateId, string password) : this(aggregateId, password, Guid.NewGuid())
        {
        }

        public GetAuthenticatedUser(string aggregateId, string password, Guid correlationId) : this(aggregateId, password, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        [JsonConstructor]
        public GetAuthenticatedUser(string aggregateId, string password, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base("User", aggregateId, aggregateId, correlationId, messageId, dateTime)
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

        public string Password { get; [Obsolete]set; }
#pragma warning disable CS8603 // Possible null reference return.
        public string aggregateId => AggregateId;
#pragma warning restore CS8603 // Possible null reference return.
    }
}
using System;

using Newtonsoft.Json;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Security.Users.Queries
{
    public class GetUser : UserQuery<IUser>
    {
        [Obsolete("Can only be used by serializers", true)]
        protected GetUser()
        {
        }

        public GetUser(string aggregateId, string userId) : this(aggregateId, userId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        public GetUser(string aggregateId, string userId, Guid correlationId) : this(aggregateId, userId, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        [JsonConstructor]
        public GetUser(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateId, userId, correlationId, messageId, dateTime)
        {
            if (string.IsNullOrWhiteSpace(aggregateId))
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }
        }

#pragma warning disable CS8603 // Possible null reference return.
        public string UserName => AggregateId;
#pragma warning restore CS8603 // Possible null reference return.
    }
}
using System;

using ExtenFlow.Messages;

namespace ExtenFlow.Security.Users.Queries
{
    public abstract class UserQuery<T> : Query<T>
    {
        [Obsolete("Can only be used by serializers", true)]
        protected UserQuery()
        {
            AggregateType = "User";
        }

        protected UserQuery(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base("User", aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
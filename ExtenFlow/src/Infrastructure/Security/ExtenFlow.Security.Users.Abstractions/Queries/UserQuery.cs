using System;

using ExtenFlow.Messages;

namespace ExtenFlow.Security.Users.Queries
{
    public abstract class UserQuery<T> : Query<T>
    {
        protected UserQuery() : this(string.Empty, string.Empty, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        protected UserQuery(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base("User", aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
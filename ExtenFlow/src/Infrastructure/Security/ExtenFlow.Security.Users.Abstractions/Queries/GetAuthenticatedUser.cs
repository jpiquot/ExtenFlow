using System;

using ExtenFlow.Messages;

using Newtonsoft.Json;

namespace ExtenFlow.Security.Users.Abstractions.Queries
{
    public class GetAuthenticatedUser : Query<IUser>
    {
        [Obsolete("Can only be used by serializers", true)]
        protected GetAuthenticatedUser()
        {
            Password = string.Empty;
        }

        public GetAuthenticatedUser(string userName, string password) : this(userName, password, Guid.NewGuid())
        {
        }

        public GetAuthenticatedUser(string userName, string password, Guid correlationId) : this(userName, password, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        [JsonConstructor]
        public GetAuthenticatedUser(string userName, string password, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base("User", userName, userName, correlationId, messageId, dateTime)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            Password = password;
        }

        public string Password { get; }
#pragma warning disable CA1303 // Do not pass literals as localized parameters
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
        public string UserName => AggregateId ?? throw new NullReferenceException(nameof(AggregateId));
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
#pragma warning restore CA1303 // Do not pass literals as localized parameters
    }
}
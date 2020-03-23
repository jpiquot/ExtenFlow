using System;

using Newtonsoft.Json;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete
#pragma warning disable CS8603 // Possible null reference return.

namespace ExtenFlow.Security.Users.Commands
{
    public class RegisterNewUser : UserCommand
    {
        [Obsolete("Can only be used by serializers", true)]
        protected RegisterNewUser()
        {
            Password = string.Empty;
        }

        public RegisterNewUser(string aggregateId, string password) : this(aggregateId, password, aggregateId, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        public RegisterNewUser(string aggregateId, string password, Guid correlationId) : this(aggregateId, password, aggregateId, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

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

        public string Password { get; [Obsolete]set; }
        public string UserName => AggregateId;
    }
}
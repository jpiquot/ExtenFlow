using System;
using System.Security.Claims;

using ExtenFlow.Messages;

using Newtonsoft.Json;

namespace ExtenFlow.Security.Users.Abstractions.Queries
{
    public class GetAuthenticatedClaimUser : Query<IUser>
    {
        [Obsolete("Can only be used by serializers", true)]
        protected GetAuthenticatedClaimUser()
        {
            Claim = new ClaimsPrincipal();
        }

        public GetAuthenticatedClaimUser(ClaimsPrincipal claim) : this(claim, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        public GetAuthenticatedClaimUser(ClaimsPrincipal claim, Guid correlationId) : this(claim, correlationId, Guid.NewGuid(), DateTimeOffset.Now)
        {
        }

        [JsonConstructor]
        public GetAuthenticatedClaimUser(ClaimsPrincipal claim, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base("User", claim?.Identity?.Name ?? string.Empty, claim?.Identity?.Name ?? string.Empty, correlationId, messageId, dateTime)
        {
            Claim = claim ?? throw new ArgumentNullException(nameof(claim));
        }

        public ClaimsPrincipal Claim { get; }
    }
}
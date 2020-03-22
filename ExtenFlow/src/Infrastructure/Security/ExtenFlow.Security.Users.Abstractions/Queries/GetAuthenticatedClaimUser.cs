using System;
using System.Security.Claims;

using Newtonsoft.Json;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Security.Users.Queries
{
    public class GetAuthenticatedClaimUser : UserQuery<IUser>
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
        public GetAuthenticatedClaimUser(ClaimsPrincipal claim, Guid correlationId, Guid messageId, DateTimeOffset dateTime)
            : base(claim?.Identity?.Name ?? string.Empty, claim?.Identity?.Name ?? string.Empty, correlationId, messageId, dateTime)
        {
            Claim = claim ?? throw new ArgumentNullException(nameof(claim));
        }

        public ClaimsPrincipal Claim { get; [Obsolete]set; }
    }
}
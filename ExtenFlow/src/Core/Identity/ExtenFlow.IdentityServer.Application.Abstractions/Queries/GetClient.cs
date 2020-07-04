using System;

using IdentityServer4.Models;

namespace ExtenFlow.IdentityServer.Application.Queries
{
    /// <summary>
    /// Class GetClient. Implements the <see cref="ClientQuery{Client}"/>
    /// </summary>
    /// <seealso cref="ClientQuery{Client}"/>
    public class GetClient : ClientQuery<Client>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetClient"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public GetClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetClient"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public GetClient(string aggregateId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
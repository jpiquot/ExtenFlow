using System;

namespace ExtenFlow.IdentityServer.Domain.Events
{
    /// <summary>
    /// Client deleted
    /// </summary>
    /// <seealso cref="ClientEvent"/>
    public class ClientRemoved : ClientEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRemoved"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public ClientRemoved()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRemoved"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public ClientRemoved(string aggregateId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
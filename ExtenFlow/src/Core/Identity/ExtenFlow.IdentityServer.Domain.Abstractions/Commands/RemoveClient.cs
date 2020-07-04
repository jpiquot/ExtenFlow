using System;

namespace ExtenFlow.IdentityServer.Domain.Commands
{
    /// <summary>
    /// Delete client command
    /// </summary>
    /// <seealso cref="ClientCommand"/>
    public class RemoveClient : ClientCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveClient"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public RemoveClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveClient"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="concurrencyCheckStamp">
        /// Concurrency stamp used for optimistic concurrency check.
        /// </param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public RemoveClient(string aggregateId, string userId, string concurrencyCheckStamp, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, concurrencyCheckStamp, correlationId, id, dateTime)
        {
        }
    }
}
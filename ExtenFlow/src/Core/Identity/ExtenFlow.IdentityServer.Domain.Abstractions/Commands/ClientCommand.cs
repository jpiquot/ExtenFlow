using System;

using ExtenFlow.Messages;

namespace ExtenFlow.IdentityServer.Domain.Commands
{
    /// <summary>
    /// Base Client command class
    /// </summary>
    /// <seealso cref="Command"/>
    public abstract class ClientCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCommand"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected ClientCommand()
        {
            AggregateType = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCommand"/> class.
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
        protected ClientCommand(string aggregateId,
                              string userId,
                              string? concurrencyCheckStamp = null,
                              string? correlationId = null,
                              string? id = null,
                              DateTimeOffset? dateTime = null)
            : base(AggregateName.Client, aggregateId, userId, concurrencyCheckStamp, correlationId, id, dateTime)
        {
        }
    }
}
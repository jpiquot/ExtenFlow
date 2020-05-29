using System;

using ExtenFlow.Infrastructure;

#pragma warning disable CA1041 // Provide ObsoleteAttribute message
#pragma warning disable CS0612 // Type or member is obsolete

namespace ExtenFlow.Messages
{
    /// <summary>
    /// The base class of all commands
    /// </summary>
    public abstract class Command : Request, ICommand
    {
        /// <summary>
        /// Command base class constructor.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected Command()
        {
            ConcurrencyCheckStamp = string.Empty;
        }

        /// <summary>
        /// Constructor for the base command class
        /// </summary>
        /// <param name="aggregateType">Type of aggregate</param>
        /// <param name="aggregateId">Aggragate Id</param>
        /// <param name="userId">The user submitting the command</param>
        /// <param name="concurrencyCheckStamp">
        /// Concurrency stamp used for optimistic concurrency check.
        /// </param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events
        /// </param>
        /// <param name="id">The Id of this command</param>
        /// <param name="dateTime">The date time of the command submission</param>
        protected Command(string aggregateType, string? aggregateId, string userId, string? concurrencyCheckStamp = null, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null) : base(aggregateType, aggregateId, userId, correlationId, id, dateTime)
        {
            ConcurrencyCheckStamp = concurrencyCheckStamp ?? Guid.NewGuid().ToBase64String();
        }

        /// <summary>
        /// Gets the concurrency stamp used for optimistic concurrency check.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        public string ConcurrencyCheckStamp { get; }
    }
}
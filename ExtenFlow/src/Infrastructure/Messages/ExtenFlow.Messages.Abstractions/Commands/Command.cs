using System;

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
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected Command()
        {
        }

        /// <summary>
        /// Constructor for the base command class
        /// </summary>
        /// <param name="aggregateType">Type of aggregate</param>
        /// <param name="aggregateId">Aggragate Id</param>
        /// <param name="concurrencyStamp">Concurrency stamp used for optimistic concurrency check.</param>
        /// <param name="userId">The user submitting the command</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events
        /// </param>
        /// <param name="messageId">The Id of this command</param>
        /// <param name="dateTime">The date time of the command submission</param>
        protected Command(string aggregateType, string? aggregateId, string? concurrencyStamp, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(aggregateType, aggregateId, userId, correlationId, messageId, dateTime)
        {
            ConcurrencyStamp = concurrencyStamp;
        }

        /// <summary>
        /// Gets the concurrency stamp used for optimistic concurrency check.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        public string? ConcurrencyStamp { get; }
    }
}
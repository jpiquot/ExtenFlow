using System;

using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Roles.Events
{
    /// <summary>
    /// Base Role event class
    /// </summary>
    /// <seealso cref="Event"/>
    public abstract class RoleEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleEvent"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected RoleEvent()
        {
            AggregateType = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        protected RoleEvent(string aggregateId, string userId, string? correlationId = null, string? id = null, DateTimeOffset? dateTime = null)
            : base(AggregateName.Role, aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
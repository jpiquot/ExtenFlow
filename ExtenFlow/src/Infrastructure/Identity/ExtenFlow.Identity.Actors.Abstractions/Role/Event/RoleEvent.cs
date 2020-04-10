using System;

using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// Base Role event class
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.Event"/>
    public abstract class RoleEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleEvent"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected RoleEvent()
        {
            AggregateType = nameof(Role);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="dateTime">The date time.</param>
        protected RoleEvent(string aggregateId, string userId, Guid correlationId, Guid messageId, DateTimeOffset dateTime) : base(nameof(Role), aggregateId, userId, correlationId, messageId, dateTime)
        {
        }
    }
}
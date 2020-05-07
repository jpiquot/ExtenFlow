using System;

using ExtenFlow.Identity.Models;
using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Users.Events
{
    /// <summary>
    /// Base User event class
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.Event"/>
    public abstract class UserEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserEvent"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        protected UserEvent()
        {
            AggregateType = nameof(User);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        protected UserEvent(string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime) : base(nameof(User), aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
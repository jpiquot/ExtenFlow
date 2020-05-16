using System;

using ExtenFlow.Identity.Roles.Models;
using ExtenFlow.Domain;

namespace ExtenFlow.Identity.Roles.Events
{
    /// <summary>
    /// Base Role event class
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.Event"/>
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
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        protected RoleEvent(string aggregateId, string userId, Guid correlationId, Guid id, DateTimeOffset dateTime) : base(nameof(Role), aggregateId, userId, correlationId, id, dateTime)
        {
        }
    }
}
﻿using System;

namespace ExtenFlow.Identity.Users.Events
{
    /// <summary>
    /// User deleted
    /// </summary>
    /// <seealso cref="UserEvent"/>
    public class UserUnregistred : UserEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserUnregistred"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public UserUnregistred()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserUnregistred"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public UserUnregistred(string aggregateId, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
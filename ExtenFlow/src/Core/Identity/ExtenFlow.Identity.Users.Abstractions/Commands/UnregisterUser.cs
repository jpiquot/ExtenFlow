using System;

namespace ExtenFlow.Identity.Users.Commands
{
    /// <summary>
    /// Delete user command
    /// </summary>
    /// <seealso cref="UserCommand"/>
    public class UnregisterUser : UserCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnregisterUser"/> class.
        /// </summary>
        /// <remarks>This constructor must not be used. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public UnregisterUser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnregisterUser"/> class.
        /// </summary>
        /// <param name="aggregateId">Aggragate Id.</param>
        /// <param name="concurrencyStamp">Concurrency stamp used for optimistic concurrency check.</param>
        /// <param name="userId">The user submitting the command.</param>
        /// <param name="correlationId">
        /// The correlation id used to chain messages, queries, commands and events.
        /// </param>
        /// <param name="id">The Id of this command.</param>
        /// <param name="dateTime">The date time of the command submission.</param>
        public UnregisterUser(string aggregateId, string concurrencyStamp, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, concurrencyStamp, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
using System;
using System.Collections.Generic;

using Dapr.Actors.Runtime;

using ExtenFlow.Domain;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Actor reminder callback notification.
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.Message"/>
    public class ActorReminderCallback : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActorReminderCallback"/> class.
        /// </summary>
        public ActorReminderCallback(Actor actor, string? reminderName, byte[]? state, TimeSpan? dueTime, TimeSpan? period)
            : base(actor.ActorName(), actor.Id.GetId(), actor.GetType().Name, Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now)
        {
            Name = reminderName ?? string.Empty;
            State = state == null ? new List<byte>() : new List<byte>(state);
            DueTime = dueTime ?? TimeSpan.Zero;
            Period = period ?? TimeSpan.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorReminderCallback"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. For serializer usage only.</remarks>
        [Obsolete("Can only be used by serializers")]
        protected ActorReminderCallback()
        {
            Name = string.Empty;
            State = new List<byte>();
            DueTime = TimeSpan.Zero;
            Period = TimeSpan.Zero;
        }

        /// <summary>
        /// Gets the reminder due time.
        /// </summary>
        /// <value>The due time.</value>
        public TimeSpan DueTime { get; }

        /// <summary>
        /// Gets the reminder name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the reminder period.
        /// </summary>
        /// <value>The period.</value>
        public TimeSpan Period { get; }

        /// <summary>
        /// Gets the reminder state.
        /// </summary>
        /// <value>The state.</value>
        public List<byte> State { get; }
    }
}
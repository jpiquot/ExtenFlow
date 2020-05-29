using System;

using Dapr.Actors.Runtime;

using ExtenFlow.Messages;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Actor timer callback notification.
    /// </summary>
    /// <seealso cref="Message"/>
    public class ActorTimerCallback : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActorTimerCallback"/> class.
        /// </summary>
        /// <param name="actor">The actor.</param>
        public ActorTimerCallback(Actor actor)
            : base(actor.ActorName(), actor.Id.GetId(), actor.GetType().Name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorTimerCallback"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. For serializer usage only.</remarks>
        [Obsolete("Can only be used by serializers")]
        protected ActorTimerCallback()
        {
        }
    }
}
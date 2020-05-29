using System;

using Dapr.Actors.Runtime;

using ExtenFlow.Messages;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Actor activation notification.
    /// </summary>
    /// <seealso cref="Message"/>
    public class ActorActivation : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActorActivation"/> class.
        /// </summary>
        /// <param name="actor">The actor.</param>
        public ActorActivation(Actor actor)
            : base(actor.ActorName(), actor.Id.GetId(), actor.GetType().Name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorActivation"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. For serializer usage only.</remarks>
        [Obsolete("Can only be used by serializers")]
        protected ActorActivation()
        {
        }
    }
}
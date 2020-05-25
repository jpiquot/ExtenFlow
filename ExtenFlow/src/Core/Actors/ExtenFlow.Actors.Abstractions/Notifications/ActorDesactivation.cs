using System;

using Dapr.Actors.Runtime;

using ExtenFlow.Domain;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Actor desactivation notification.
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.Message"/>
    public class ActorDesactivation : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDesactivation"/> class.
        /// </summary>
        /// <param name="actor">The actor.</param>
        public ActorDesactivation(Actor actor)
            : base(actor.ActorName(), actor.Id.GetId(), actor.GetType().Name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDesactivation"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. For serializer usage only.</remarks>
        [Obsolete("Can only be used by serializers")]
        protected ActorDesactivation()
        {
        }
    }
}
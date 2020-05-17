using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Client;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Class ActorSystem. Implements the <see cref="ExtenFlow.Actors.IActorSystem"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Actors.IActorSystem"/>
    public class ActorSystem : IActorSystem
    {
        /// <summary>
        /// Creates the specified identifier.
        /// </summary>
        /// <typeparam name="TActor">The interface type of the actor.</typeparam>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>The actor interface</returns>
        public TActor Create<TActor, TId>(TId id) where TActor : IActor
            => (TActor)Create(typeof(TActor), id);

        /// <summary>
        /// Creates the specified identifier.
        /// </summary>
        /// <typeparam name="TActor">The interface type of the actor.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>The actor interface</returns>
        public TActor Create<TActor>(string id) where TActor : IActor
            => (TActor)Create(typeof(TActor), id ?? string.Empty);

        /// <summary>
        /// Creates the specified actor type.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="actorType">Type of the actor.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>The actor interface</returns>
        public object Create<TId>(Type actorType, TId id)
            => ActorProxy.Create(new ActorId(id?.ToString()), actorType, actorType?.Name.Substring(1));

        /// <summary>
        /// Creates the specified actor type with a string key type.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="actorType">Type of the actor.</param>
        /// <returns>The actor interface</returns>
        public object Create<TId>(Type actorType)
            => Create(actorType, actorType?.Name.Substring(1));
    }
}
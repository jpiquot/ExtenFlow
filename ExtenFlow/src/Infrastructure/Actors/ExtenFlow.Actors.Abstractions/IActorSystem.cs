using System;

using Dapr.Actors;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Interface IActorSystem
    /// </summary>
    public interface IActorSystem
    {
        /// <summary>
        /// Creates the specified identifier.
        /// </summary>
        /// <typeparam name="TActor">The interface type of the actor.</typeparam>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>The actor interface</returns>
        TActor Create<TActor, TId>(TId id) where TActor : IActor;

        /// <summary>
        /// Creates the specified identifier.
        /// </summary>
        /// <typeparam name="TActor">The interface type of the actor.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>The actor interface</returns>
        TActor Create<TActor>(string id) where TActor : IActor;

        /// <summary>
        /// Creates the specified actor type.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="actorType">Type of the actor.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>The actor interface</returns>
        object Create<TId>(Type actorType, TId id);

        /// <summary>
        /// Creates the specified actor type with a string key type.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="actorType">Type of the actor.</param>
        /// <returns>The actor interface</returns>
        object Create<TId>(Type actorType);
    }
}
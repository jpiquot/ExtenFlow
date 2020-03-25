using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Security.DaprStore.Actors
{
    /// <summary>
    /// The user normalized name index
    /// </summary>
    /// <seealso cref="Dapr.Actors.Runtime.Actor"/>
    /// <seealso cref="ExtenFlow.Security.DaprStore.Actors.IUserNormalizedNameIndexActor"/>
    public class UserNormalizedNameIndexActor : Actor, IUserNormalizedNameIndexActor
    {
        private string? _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNormalizedNameIndexActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserNormalizedNameIndexActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Gets the identifier for the normalized name.
        /// </summary>
        /// <returns></returns>
        public Task<string?> GetId() => Task.FromResult(_id);

        /// <summary>
        /// Indexes the specified identifier for the normalized name.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Task Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException<string>(new NullReferenceException(nameof(_id)));
            }
            _id = id;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Removes the specified identifier from the index.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Task Remove(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException<string>(new NullReferenceException(nameof(_id)));
            }
            if (id != _id)
            {
                return Task.FromException<string>(new InvalidOperationException(nameof(_id)));
            }
            _id = null;
            return Task.CompletedTask;
        }
    }
}
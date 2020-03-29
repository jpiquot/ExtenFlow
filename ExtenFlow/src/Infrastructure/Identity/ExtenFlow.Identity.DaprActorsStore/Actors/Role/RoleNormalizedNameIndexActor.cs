using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The Role normalized name index
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IRoleNormalizedNameIndexActor"/>
    public class RoleNormalizedNameIndexActor : Actor, IRoleNormalizedNameIndexActor
    {
        private string? _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNormalizedNameIndexActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleNormalizedNameIndexActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
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
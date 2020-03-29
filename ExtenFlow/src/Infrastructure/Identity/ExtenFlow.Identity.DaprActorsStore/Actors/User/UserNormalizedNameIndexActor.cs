using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user normalized name index
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserNormalizedNameIndexActor"/>
    public class UserNormalizedNameIndexActor : Actor, IUserNormalizedNameIndexActor
    {
        private const string _stateName = "UserNormalizedNameIndex";
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
            return StateManager.SetStateAsync(_stateName, _id);
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
            return StateManager.SetStateAsync(_stateName, _id);
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _id = await StateManager.GetStateAsync<string?>(_stateName);
            await base.OnActivateAsync();
        }
    }
}
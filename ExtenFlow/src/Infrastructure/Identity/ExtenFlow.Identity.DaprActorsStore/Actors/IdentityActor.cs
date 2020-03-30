using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Identity.DaprActorsStore.Actors
{
    /// <summary>
    /// Base class for identity actors
    /// </summary>
    /// <seealso cref="Actor"/>
    public abstract class IdentityActor<T> : Actor where T : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityActor{T}"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        protected IdentityActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        private Nullable<T> _state;
        private string? _stateName;

        /// <summary>
        /// Gets the name of the state.By default it's the actor class name without the 'Actor' word.
        /// </summary>
        /// <value>The name of the state.</value>
        protected virtual string StateName => _stateName ?? (_stateName = GetType().Name.Replace("Actor", string.Empty));

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<Nullable<T>>(GetType().Name);
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Sets the state by calling the state manager <see
        /// cref="ActorStateManager.SetStateAsync{T}(string, T,
        /// System.Threading.CancellationToken)"/> method.
        /// </summary>
        protected virtual Task SetState() => StateManager.SetStateAsync<Nullable<T>>(StateName, _state);
    }
}
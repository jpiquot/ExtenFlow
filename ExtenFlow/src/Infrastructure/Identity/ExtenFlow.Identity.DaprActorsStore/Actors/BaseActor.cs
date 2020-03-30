using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// Base class for identity actors
    /// </summary>
    /// <seealso cref="Actor"/>
    public abstract class BaseActor<T> : Actor where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseActor{T}"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        protected BaseActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// The state
        /// </summary>
        protected T? _state;

        private string? _stateName;

        /// <summary>
        /// Gets the name of the state.By default it's the actor class name without the 'Actor' word.
        /// </summary>
        /// <value>The name of the state.</value>
        protected virtual string StateName => _stateName ?? (_stateName = GetType().Name.Replace("Actor", string.Empty));

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        protected virtual T State => _state ?? (_state = new T());

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<T?>(GetType().Name);
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Sets the state by calling the state manager <see
        /// cref="ActorStateManager.SetStateAsync{T}(string, T,
        /// System.Threading.CancellationToken)"/> method.
        /// </summary>
        protected virtual Task SetState() => StateManager.SetStateAsync(StateName, _state);

        /// <summary>
        /// Sets the state by calling the state manager <see
        /// cref="ActorStateManager.SetStateAsync{T}(string, T,
        /// System.Threading.CancellationToken)"/> method. If succeeded, the in memory State value
        /// is initialized with the new state.
        /// </summary>
        /// <param name="newState">The state to save</param>
        protected virtual async Task SetState(T? newState)
        {
            await StateManager.SetStateAsync(StateName, newState);
            _state = newState;
        }
    }
}
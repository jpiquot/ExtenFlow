using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Base actor class
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <seealso cref="Actor"/>
    public abstract class ActorBase<TState> : Actor
        where TState : class
    {
        private string? _stateName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorBase{TState}"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">Id for the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        protected ActorBase(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        protected TState? State { get; set; }

        /// <summary>
        /// Gets the name of the state.
        /// </summary>
        /// <value>The name of the state.</value>
        protected string StateName => _stateName ?? (_stateName = this.ActorName());

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            await ReadStateData();
        }

        /// <summary>
        /// Gets the actor state.
        /// </summary>
        protected virtual async Task ReadStateData()
        {
            try
            {
                State = await StateManager.GetStateAsync<TState>(this.ActorName());
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (KeyNotFoundException)
            {
                State = null;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        /// <summary>
        /// Saves the actor state.
        /// </summary>
        /// <returns></returns>
        protected virtual Task SetStateData()
            => (State == null) ? StateManager.RemoveStateAsync(StateName) : StateManager.SetStateAsync(StateName, State);
    }
}
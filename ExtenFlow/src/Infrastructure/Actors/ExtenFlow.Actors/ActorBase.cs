using System;
using System.Collections.Generic;
using System.Globalization;
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
    public abstract class ActorBase<TState> : Actor, IRemindable, IBaseActor
        where TState : class, new()
    {
        private TState? _state;
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
        protected TState State
            => _state ?? (_state = new TState());

        /// <summary>
        /// Gets the name of the state.
        /// </summary>
        /// <value>The name of the state.</value>
        protected string StateName => _stateName ?? (_stateName = this.ActorName());

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <returns></returns>
        public Task<object> GetStateValue()
            // The actor state has not been initialized for actor {0} with Id '{1}'.
            => (_state != null) ?
                    Task.FromResult<object>(State) :
                    Task.FromException<object>(new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.ActorStateNotInitialized, this.ActorName(), Id.GetId())));

        /// <summary>
        /// Determines whether this instance is initialized.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> IsInitialized() => Task.FromResult(_state != null);

        /// <inheriteddoc/>
        public virtual Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
            => Task.CompletedTask;

        /// <summary>
        /// Registers the reminder.
        /// </summary>
        /// <param name="dueTime">The due time.</param>
        /// <param name="period">The period.</param>
        /// <param name="state">The reminder state</param>
        /// <param name="reminderName">The reminder name</param>
        public virtual Task RegisterReminder(TimeSpan dueTime, TimeSpan period, byte[]? state = null, string? reminderName = null)
            => RegisterReminderAsync(reminderName ?? this.ActorName(), state, dueTime, period);

        /// <summary>
        /// Clears the state.
        /// </summary>
        protected void ClearState() => _state = null;

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
                _state = await StateManager.GetStateAsync<TState>(this.ActorName());
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (KeyNotFoundException)
            {
                _state = null;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        /// <summary>
        /// Saves the actor state.
        /// </summary>
        /// <returns></returns>
        protected virtual Task SetStateData()
            => (_state == null) ? StateManager.RemoveStateAsync(StateName) : StateManager.SetStateAsync(StateName, State);

        /// <summary>
        /// Clears the state.
        /// </summary>
        protected bool StateIsNull() => _state == null;
    }
}
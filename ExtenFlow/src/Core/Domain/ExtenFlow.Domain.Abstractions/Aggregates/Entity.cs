using System;
using System.Threading.Tasks;

using ExtenFlow.Domain.Exceptions;
using ExtenFlow.Messages;

#pragma warning disable CA1716 // Identifiers should not match keywords

namespace ExtenFlow.Domain.Aggregates
{
    /// <summary>
    /// Class Entity. Implements the <see cref="ExtenFlow.Domain.Aggregates.IEntity"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.Aggregates.IEntity"/>
    public abstract class Entity<T> : IEntity
    {
        private bool _hasData;
        private bool _initialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{T}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="repository">The repository.</param>
        /// <exception cref="ArgumentNullException">id</exception>
        /// <exception cref="ArgumentNullException">repository</exception>
        /// <exception cref="ArgumentNullException">name</exception>
        protected Entity(string name, string id, IRepository repository)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            EntityName = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets aggregate name.
        /// </summary>
        /// <value>The name.</value>
        public string EntityName { get; }

        /// <summary>
        /// Gets the aggregate identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has data.
        /// </summary>
        /// <value><c>true</c> if this instance has data; otherwise, <c>false</c>.</value>
        protected bool HasData => (_initialized) ? _hasData : throw new EntityStateNotInitializedException(this, nameof(HasData));

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>The repository.</value>
        protected IRepository Repository { get; }

        /// <summary>
        /// Handles the events.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public abstract Task HandleEvent(IEvent @event);

        /// <summary>
        /// Initializes the state.
        /// </summary>
        public async Task InitializeState()
        {
            if (!_initialized)
            {
                (bool succeded, T state) = await Repository.TryGetData<T>(EntityName);
                if (succeded)
                {
                    SetValues(state);
                    _hasData = true;
                }
                else
                {
                    ClearValues();
                }
                _initialized = true;
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <exception cref="ExtenFlow.Domain.Exceptions.EntityStateNotInitializedException"></exception>
        public async Task Save()
        {
            if (!_initialized)
            {
                throw new EntityStateNotInitializedException(this);
            }
            if (_hasData)
            {
                await Repository.SetData(EntityName, GetState());
            }
            else
            {
                await Repository.RemoveData(EntityName);
            }
        }

        /// <summary>
        /// Checks the exists.
        /// </summary>
        /// <exception cref="ExtenFlow.Domain.Exceptions.EntityNotFoundException"></exception>
        protected void CheckExists()
        {
            if (!HasData)
            {
                throw new EntityNotFoundException(this);
            }
        }

        /// <summary>
        /// Clears the instance values.
        /// </summary>
        protected virtual void ClearValues() => _hasData = false;

        /// <summary>
        /// Gets the state object.
        /// </summary>
        /// <returns>The state object initialized with the instance values.</returns>
        protected abstract T GetState();

        /// <summary>
        /// Sets the data values from the persisted state object.
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void SetValues(T state);
    }
}
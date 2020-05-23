using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using ExtenFlow.Domain;

#pragma warning disable CS8604 // Possible null reference argument.

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Class ActorRepository. Implements the <see cref="ExtenFlow.Domain.IRepository"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.IRepository"/>
    public class ActorRepository : IRepository
    {
        private readonly IActorStateManager _stateManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorRepository"/> class.
        /// </summary>
        /// <param name="stateManager">The state manager.</param>
        public ActorRepository(IActorStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        /// <summary>
        /// Reads the data from the storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public Task<T> GetData<T>(string name) => _stateManager.GetStateAsync<T>(name);

        /// <summary>
        /// Determines whether the specified storage name has data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<bool> HasData(string name) => _stateManager.ContainsStateAsync(name);

        /// <summary>
        /// Clears the data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Task.</returns>
        public Task RemoveData(string name) => _stateManager.RemoveStateAsync(name);

        /// <summary>
        /// Writes the data to the storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task SetData<T>(string name, T value) => _stateManager.SetStateAsync(name, value);

        /// <summary>
        /// Reads the data from the storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public async Task<(bool, T)> TryGetData<T>(string name)
        {
            ConditionalValue<T> value = await _stateManager.TryGetStateAsync<T>(name);
            return (value.HasValue, (value.HasValue) ? value.Value : default);
        }
    }
}
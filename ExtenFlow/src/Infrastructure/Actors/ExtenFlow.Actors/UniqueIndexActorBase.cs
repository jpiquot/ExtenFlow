using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;
using ExtenFlow.Services;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// The index actor base class
    /// </summary>
    /// <seealso cref="Actor"/>
    public abstract class UniqueIndexActorBase : ActorBase<Dictionary<string, string>>, IUniqueIndexService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueIndexActorBase"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="stateManager">The state manager.</param>
        protected UniqueIndexActorBase(ActorService actorService, ActorId actorId, IActorStateManager? stateManager) : base(actorService, actorId, stateManager)
        {
        }

        /// <summary>
        /// Adds a new item with the specified identifier.
        /// </summary>
        /// <param name="key">The key value to index.</param>
        /// <param name="id">The identifier.</param>
        public Task Add(string key, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException(new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace));
            }
            if (State == null)
            {
                State = new Dictionary<string, string>();
            }
            if (State.TryGetValue(key, out string? currentId))
            {
                if (!id.Equals(currentId, StringComparison.InvariantCulture))
                {
                    // Item with Key='{0}' already exist in unique index {1} with another Id='{2}'.
                    return Task.FromException(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ItemExistInIndex, id, Id.GetId(), currentId)));
                }
            }
            State.Add(key, id);
            return SetStateData();
        }

        /// <summary>
        /// Check if an item with the specified identifier exists.
        /// </summary>
        /// <param name="key">The key value to index.</param>
        /// <returns>True if the id exists, else false.</returns>
        public Task<bool> Exist(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Task.FromException<bool>(new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace));
            }
            if (State == null || !State.ContainsKey(key))
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="key">The key value to index.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<string?> GetIdentifier(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Task.FromException<string?>(new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace));
            }
            string? id = null;
            if (State?.TryGetValue(key, out id) == false)
            {
                return Task.FromException<string?>(new KeyNotFoundException(key));
            }
            return Task.FromResult(id);
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<string?> GetKey(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException<string?>(new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace));
            }
            if (State != null)
            {
                foreach (KeyValuePair<string, string> pair in State)
                {
                    if (pair.Value.Equals(id, StringComparison.InvariantCulture))
                    {
                        return Task.FromResult<string?>(pair.Key);
                    }
                }
            }
            return Task.FromException<string?>(new KeyNotFoundException(id));
        }

        /// <summary>
        /// Removes a existing item with the specified identifier.
        /// </summary>
        /// <param name="key">The key value to index.</param>
        public Task Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Task.FromException(new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace));
            }
            if (State == null || !State.ContainsKey(key))
            {
                return Task.FromException(new KeyNotFoundException(key));
            }
            State.Remove(key);
            if (State.Count == 0)
            {
                State = null;
            }
            return SetStateData();
        }
    }
}
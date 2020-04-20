using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors.Exceptions;
using ExtenFlow.Services;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// The index actor base class
    /// </summary>
    /// <seealso cref="Actor"/>
    public class IndexActor : ActorBase<Dictionary<string, HashSet<string>>>, IIndexActor, IIndexService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="stateManager">The state manager.</param>
        public IndexActor(ActorService actorService, ActorId actorId, IActorStateManager? stateManager) : base(actorService, actorId, stateManager)
        {
        }

        /// <summary>
        /// Adds a new item with the specified identifier.
        /// </summary>
        /// <param name="key">The key value to index.</param>
        /// <param name="id">The identifier.</param>
        public async Task Add(string key, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace);
            }
            var oldkey = await GetKey(id);
            if (oldkey != null && oldkey != key)
            {
                // Item with Id='{0}' already exist in unique index {1} with another Key='{2}'
                throw new DuplicateIdentifierInIndexException(CultureInfo.CurrentCulture, id, Id.GetId(), oldkey);
            }
            if (!State.TryGetValue(key, out HashSet<string>? currentIds))
            {
                currentIds = new HashSet<string>();
                State.Add(key, currentIds);
            }
            currentIds.Add(id);
            await SetStateData();
        }

        /// <summary>
        /// Check if an item with the specified identifier exists.
        /// </summary>
        /// <param name="key">The key value to index.</param>
        /// <param name="id"></param>
        /// <returns>True if the id exists, else false.</returns>
        public Task<bool> Exist(string key, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException<bool>(new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace));
            }
            if (StateIsNull() || !State.ContainsKey(key))
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(State.Where(p => p.Key == key).Any(p => p.Key.Contains(id, StringComparison.InvariantCulture)));
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="key">The key value to index.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IList<string>> GetIdentifiers(string key)
        {
            if (!StateIsNull() && State.TryGetValue(key, out HashSet<string>? ids))
            {
                if (ids != null)
                {
                    return Task.FromResult<IList<string>>(ids.ToList());
                }
            }
            return Task.FromResult<IList<string>>(new List<string>());
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
            if (!StateIsNull())
            {
                foreach (KeyValuePair<string, HashSet<string>> pair in State)
                {
                    if (pair.Value != null && pair.Value.Contains(id))
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
        /// <param name="id"></param>
        public Task Remove(string key, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException(new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace, nameof(id)));
            }
            if (StateIsNull())
            {
                return Task.FromException(new KeyNotFoundException(key));
            }
            if (!StateIsNull() && State.TryGetValue(key, out HashSet<string>? ids))
            {
                if (ids != null && ids.Contains(id))
                {
                    ids.Remove(id);
                    return Task.CompletedTask;
                }
            }
            State.Remove(key);
            if (State.Count == 0)
            {
                ClearState();
            }
            return SetStateData();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    public class CollectionActor : ActorBase<HashSet<string>>, ICollectionActor, ICollectionService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="stateManager">The state manager.</param>
        public CollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? stateManager) : base(actorService, actorId, stateManager)
        {
        }

        /// <summary>
        /// Adds a new item with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Task Add(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException(new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace));
            }
            else if (State.Contains(id))
            {
                // Item with Id='{0}' already exist in collection {1}.
                return Task.FromException(new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ItemExistInCollection, id, Id.GetId())));
            }
            State.Add(id);
            return SetStateData();
        }

        /// <summary>
        /// Gets all the ids.
        /// </summary>
        /// <returns>Task&lt;IList&lt;System.String&gt;&gt;.</returns>
        public Task<IList<string>> All() => Task.FromResult<IList<string>>(State.ToList());

        /// <summary>
        /// Check if an item with the specified identifier exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>True if the id exists, else false.</returns>
        public Task<bool> Exist(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException<bool>(new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace));
            }
            if (State == null || !State.Contains(id))
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        /// <summary>
        /// Removes a existing item with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Task Remove(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Task.FromException(new ArgumentException(Properties.Resources.IdIsNullEmptyOrWhiteSpace));
            }
            if (State == null || !State.Contains(id))
            {
                return Task.FromException(new KeyNotFoundException(id));
            }
            State.Remove(id);
            if (State.Count == 0)
            {
                ClearState();
            }
            return SetStateData();
        }
    }
}
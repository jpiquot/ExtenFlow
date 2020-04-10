﻿using System;
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
    public abstract class CollectionActorBase : ActorBase<HashSet<string>>, ICollectionService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionActorBase"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="stateManager">The state manager.</param>
        protected CollectionActorBase(ActorService actorService, ActorId actorId, IActorStateManager? stateManager) : base(actorService, actorId, stateManager)
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
            if (State == null)
            {
                State = new HashSet<string>();
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
                State = null;
            }
            return SetStateData();
        }
    }
}
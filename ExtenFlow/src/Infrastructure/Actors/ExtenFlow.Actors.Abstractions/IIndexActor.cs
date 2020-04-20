using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Unique index actor
    /// </summary>
    public interface IIndexActor : IActor
    {
        /// <summary>
        /// Adds a new item with the specified identifier.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <param name="id">The key identifier.</param>
        Task Add(string key, string id);

        /// <summary>
        /// Check if an item with the specified identifier and key exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="identifier">The identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> Exist(string key, string identifier);

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<IList<string>> GetIdentifiers(string key);

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<string?> GetKey(string id);

        /// <summary>
        /// Removes a existing item with the specified key/identifier.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <param name="id"></param>
        Task Remove(string key, string id);
    }
}
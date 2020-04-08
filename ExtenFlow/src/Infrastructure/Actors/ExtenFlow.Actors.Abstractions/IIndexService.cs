using System.Threading.Tasks;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Service to index key/ids
    /// </summary>
    public interface IIndexService
    {
        /// <summary>
        /// Adds a new item with the specified identifier.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <param name="id">The key identifier.</param>
        Task Add(string key, string id);

        /// <summary>
        /// Check if an item with the specified identifier exists.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <param name="id">The key identifier.</param>
        /// <returns>True if the id exists, else false.</returns>
        Task<bool> Exist(string key, string id);

        /// <summary>
        /// Removes a existing item with the specified identifier.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <param name="id">The key identifier.</param>
        Task Remove(string key, string id);
    }
}
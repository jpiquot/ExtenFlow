using System.Threading.Tasks;

namespace ExtenFlow.Services
{
    /// <summary>
    /// Service to index key/ids
    /// </summary>
    public interface IUniqueIndexService
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
        /// <returns>True if the id exists, else false.</returns>
        Task<bool> Exist(string key);

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<string?> GetIdentifier(string key);

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<string?> GetKey(string id);

        /// <summary>
        /// Removes a existing item with the specified identifier.
        /// </summary>
        /// <param name="key">key to index</param>
        Task Remove(string key);
    }
}
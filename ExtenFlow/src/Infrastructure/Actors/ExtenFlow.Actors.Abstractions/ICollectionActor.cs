using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Actor to manage collection of ids.
    /// </summary>
    public interface ICollectionActor : IActor
    {
        /// <summary>
        /// Adds a new item with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task Add(string id);

        /// <summary>
        /// Gets the ids of all roles.
        /// </summary>
        /// <returns>IList&lt;System.String&gt;.</returns>
        Task<IList<string>> All();

        /// <summary>
        /// Check if an item with the specified identifier exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>True if the id exists, else false.</returns>
        Task<bool> Exist(string id);

        /// <summary>
        /// Removes a existing item with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        Task Remove(string id);
    }
}
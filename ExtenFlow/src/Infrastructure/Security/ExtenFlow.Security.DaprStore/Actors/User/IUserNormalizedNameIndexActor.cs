using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Security.DaprStore.Actors
{
    /// <summary>
    /// User normalized name index
    /// </summary>
    /// <seealso cref="Dapr.Actors.IActor"/>
    /// <seealso cref="Dapr.Actors.Runtime.IRemindable"/>
    public interface IUserNormalizedNameIndexActor : IActor
    {
        /// <summary>
        /// Gets the identifier for the normalized name.
        /// </summary>
        /// <returns></returns>
        Task<string?> GetId();

        /// <summary>
        /// Indexes the specified identifier for the normalized name.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task Index(string id);

        /// <summary>
        /// Removes the specified identifier from the index.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task Remove(string id);
    }
}
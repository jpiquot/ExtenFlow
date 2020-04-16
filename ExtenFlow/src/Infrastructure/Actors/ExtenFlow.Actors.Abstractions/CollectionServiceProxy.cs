using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using ExtenFlow.Services;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// The collection proxy
    /// </summary>
    /// <seealso cref="ICollectionService"/>
    public class CollectionServiceProxy : ICollectionService
    {
        private readonly string _collectionName;
        private ICollectionActor? _actor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionServiceProxy"/> class.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        public CollectionServiceProxy(string collectionName) => _collectionName = collectionName;

        /// <summary>
        /// Gets the proxy service.
        /// </summary>
        /// <value>The proxy service.</value>
        protected ICollectionActor ProxyService => _actor ?? (_actor = ActorProxy.Create<ICollectionActor>(new ActorId(_collectionName), "CollectionActor"));

        /// <summary>
        /// Adds a new item with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Task Add(string id) => ProxyService.Add(id);

        /// <summary>
        /// Check if an item with the specified identifier exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>True if the id exists, else false.</returns>
        public Task<bool> Exist(string id) => ProxyService.Exist(id);

        /// <summary>
        /// Removes a existing item with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Task Remove(string id) => ProxyService.Remove(id);
    }
}
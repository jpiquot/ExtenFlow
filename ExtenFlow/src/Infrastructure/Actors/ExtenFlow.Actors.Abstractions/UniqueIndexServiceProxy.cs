using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using ExtenFlow.Services;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// The collection proxy
    /// </summary>
    /// <seealso cref="IUniqueIndexService"/>
    public class UniqueIndexServiceProxy : IUniqueIndexService
    {
        private readonly string _collectionName;
        private readonly string _propertyName;
        private IUniqueIndexActor? _actor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueIndexServiceProxy"/> class.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="propertyName"></param>
        public UniqueIndexServiceProxy(string collectionName, string propertyName)
        {
            _collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }

        /// <summary>
        /// Gets the proxy service.
        /// </summary>
        /// <value>The proxy service.</value>
        protected IUniqueIndexActor ProxyService => _actor ?? (_actor = ActorProxy.Create<IUniqueIndexActor>(new ActorId($"{_collectionName}-[{_propertyName}]"), "UniqueIndexActor"));

        /// <summary>
        /// Adds a new item with the specified identifier.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <param name="id">The key identifier.</param>
        /// <returns>Task.</returns>
        public Task Add(string key, string id) => ProxyService.Add(key, id);

        /// <summary>
        /// Check if an item with the specified identifier exists.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <returns>True if the id exists, else false.</returns>
        public Task<bool> Exist(string key) => ProxyService.Exist(key);

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
        public Task<string?> GetIdentifier(string key) => ProxyService.GetIdentifier(key);

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
        public Task<string?> GetKey(string id) => ProxyService.GetKey(id);

        /// <summary>
        /// Removes a existing item with the specified identifier.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <returns>Task.</returns>
        public Task Remove(string key) => ProxyService.Remove(key);
    }
}
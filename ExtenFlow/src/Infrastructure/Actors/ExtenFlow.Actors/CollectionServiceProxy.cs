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
    /// <typeparam name="T">The collection actor type</typeparam>
    /// <seealso cref="ICollectionService"/>
    public class CollectionServiceProxy<T> : ICollectionService where T : IActor, ICollectionService
    {
        private readonly string _collectionName;
        private ICollectionService? _actor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionServiceProxy{T}"/> class.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        public CollectionServiceProxy(string collectionName) => _collectionName = collectionName;

        /// <summary>
        /// Gets the proxy service.
        /// </summary>
        /// <value>The proxy service.</value>
        protected ICollectionService ProxyService => _actor ?? (_actor = ActorProxy.Create<T>(new ActorId(_collectionName), GetActorName()));

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

        private static string GetActorName()
        {
            string name = typeof(T).Name;
            if (name?[0] != 'I')
            {
                throw new InvalidOperationException(Properties.Resources.ActorTypeNotInterface);
            }
            return name.Substring(1);
        }
    }
}
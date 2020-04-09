using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;
using ExtenFlow.Services;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Unique index service proxy
    /// </summary>
    /// <typeparam name="T">The interface type of the unique index service actor.</typeparam>
    /// <seealso cref="IUniqueIndexService"/>
    public class UniqueIndexServiceProxy<T> : IUniqueIndexService where T : IActor, IUniqueIndexService
    {
        private readonly string _collectionName;
        private IUniqueIndexService? _actor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueIndexServiceProxy{T}"/> class.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        public UniqueIndexServiceProxy(string collectionName) => _collectionName = collectionName;

        /// <summary>
        /// Gets the proxy service.
        /// </summary>
        /// <value>The proxy service.</value>
        protected IUniqueIndexService ProxyService => _actor ?? (_actor = ActorProxy.Create<T>(new ActorId(_collectionName), GetActorName()));

        /// <summary>
        /// Adds a new item with the specified key/identifier.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <param name="id">The key identifier.</param>
        /// <returns></returns>
        public Task Add(string key, string id) => ProxyService.Add(key, id);

        /// <summary>
        /// Check if an item with the specified key exists.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <returns>True if the id exists, else false.</returns>
        public Task<bool> Exist(string key) => ProxyService.Exist(key);

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The identifier value</returns>
        public Task<string?> GetIdentifier(string key) => throw new NotImplementedException();

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="id">The key.</param>
        /// <returns>The key value</returns>
        public Task<string?> GetKey(string id) => throw new NotImplementedException();

        /// <summary>
        /// Removes a existing item with the specified key.
        /// </summary>
        /// <param name="key">key to index</param>
        /// <returns></returns>
        public Task Remove(string key) => ProxyService.Remove(key);

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
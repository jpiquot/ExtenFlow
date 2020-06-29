using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace ExtenFlow.Identity.IdentityServer.Stores
{
    /// <summary>
    /// Class DaprResourceStore. Implements the <see cref="IdentityServer4.Stores.IResourceStore"/>
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.IResourceStore"/>
    public class DaprResourceStore : IResourceStore
    {
        /// <summary>
        /// Gets API resources by API resource name.
        /// </summary>
        /// <param name="apiResourceNames">The API resource names.</param>
        /// <returns>Task&lt;IEnumerable&lt;ApiResource&gt;&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames) => throw new NotImplementedException();

        /// <summary>
        /// Gets API resources by scope name.
        /// </summary>
        /// <param name="scopeNames">The scope names.</param>
        /// <returns>Task&lt;IEnumerable&lt;ApiResource&gt;&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames) => throw new NotImplementedException();

        /// <summary>
        /// Gets API scopes by scope name.
        /// </summary>
        /// <param name="scopeNames">The scope names.</param>
        /// <returns>Task&lt;IEnumerable&lt;ApiScope&gt;&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames) => throw new NotImplementedException();

        /// <summary>
        /// Gets identity resources by scope name.
        /// </summary>
        /// <param name="scopeNames">The scope names.</param>
        /// <returns>Task&lt;IEnumerable&lt;IdentityResource&gt;&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames) => throw new NotImplementedException();

        /// <summary>
        /// Gets all resources.
        /// </summary>
        /// <returns>Task&lt;Resources&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Resources> GetAllResourcesAsync() => throw new NotImplementedException();
    }
}
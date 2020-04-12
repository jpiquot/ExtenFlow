using System;
using System.Threading;
using System.Threading.Tasks;

using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Roles.Stores;
using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Roles
{
    /// <summary>
    /// The Dapr role store
    /// </summary>
    public class RoleStore : IRoleStore
    {
        private readonly IdentityErrorDescriber _describer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleStore"/> class.
        /// </summary>
        /// <param name="describer">The describer.</param>
        public RoleStore(IdentityErrorDescriber? describer = null)
        {
            _describer = describer ?? new IdentityErrorDescriber();
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken) => throw new NotImplementedException();

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken) => throw new NotImplementedException();

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose() { }

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;Role&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken) => throw new NotImplementedException();

        /// <summary>
        /// Finds the by name asynchronous.
        /// </summary>
        /// <param name="normalizedRoleName">Name of the normalized role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;Role&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) => throw new NotImplementedException();

        /// <summary>
        /// Gets the normalized role name asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken) => throw new NotImplementedException();

        /// <summary>
        /// Gets the role identifier asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken) => throw new NotImplementedException();

        /// <summary>
        /// Gets the role name asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken) => throw new NotImplementedException();

        /// <summary>
        /// Sets the normalized role name asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="normalizedName">Name of the normalized.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken) => throw new NotImplementedException();

        /// <summary>
        /// Sets the role name asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken) => throw new NotImplementedException();

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
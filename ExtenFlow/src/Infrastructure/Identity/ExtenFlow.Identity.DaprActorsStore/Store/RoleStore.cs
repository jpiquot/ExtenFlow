using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore.Store
{
    public class RoleStore : RoleStoreBase<Role, Guid, UserRole, RoleClaim>,
        IQueryableRoleStore<Role>,
        IRoleClaimStore<Role>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleStore"/> class.
        /// </summary>
        public RoleStore() : this(new IdentityErrorDescriber())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleStore"/> class.
        /// </summary>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public RoleStore(IdentityErrorDescriber describer) : base(describer)
        {
        }

        public Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(role);
            CheckValid(claim);
            return GetRoleClaimsCollectionActor().Create(CreateRoleClaim(role, claim));
        }

        /// <summary>
        /// Creates the specified <paramref name="role"/> in the user store.
        /// </summary>
        /// <param name="role">The user to create.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see
        /// cref="IdentityResult"/> of the creation operation.
        /// </returns>
        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(role);
            IdentityResult result = await GetRoleCollectionActor().Create(role);
            if (result.Succeeded)
            {
                role.Copy(await GetRoleActor(role.Id).GetRole());
            }
            return result;
        }

        /// <summary>
        /// Deletes the specified <paramref name="role"/> from the user store.
        /// </summary>
        /// <param name="role">The user to delete.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see
        /// cref="IdentityResult"/> of the update operation.
        /// </returns>
        public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(role);

            return GetRoleCollectionActor().Delete(role.Id, role.ConcurrencyStamp);
        }

        public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken) => throw new NotImplementedException();

        public Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken) => throw new NotImplementedException();

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken) => throw new NotImplementedException();

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken) => throw new NotImplementedException();

        public Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken) => throw new NotImplementedException();

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken) => throw new NotImplementedException();

        /// <summary>
        /// Updates the specified <paramref name="role"/> in the user store.
        /// </summary>
        /// <param name="role">The user to update.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see
        /// cref="IdentityResult"/> of the update operation.
        /// </returns>
        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(role);
            IdentityResult result = await GetRoleCollectionActor().Update(role);
            if (result.Succeeded)
            {
                role.Copy(await GetRoleActor(role.Id).GetRole());
            }
            return result;
        }

        Task<Role> IRoleStore<Role>.FindByIdAsync(string roleId, CancellationToken cancellationToken) => throw new NotImplementedException();

        Task<Role> IRoleStore<Role>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) => throw new NotImplementedException();

        private void CheckValid(Role? role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (role.Id == default)
            {
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Resource.RoleIdNotDefined));
            }
        }

        private void CheckValid(Claim? claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            if (string.IsNullOrWhiteSpace(claim.Type))
            {
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Resource.ClaimTypeNotDefined));
            }
        }
    }
}
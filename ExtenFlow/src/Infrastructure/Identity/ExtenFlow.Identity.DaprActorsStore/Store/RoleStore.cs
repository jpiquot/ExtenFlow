using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Properties;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The Dapr role store
    /// </summary>
    /// <seealso cref="RoleStoreBase{Role, Guid, UserRole, RoleClaim}"/>
    /// <seealso cref="IQueryableRoleStore{Role}"/>
    /// <seealso cref="IRoleClaimStore{Role}"/>
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

        /// <summary>
        /// A navigation property for the roles the store contains.
        /// </summary>
        public override IQueryable<Role> Roles
            => RoleList()
                .GetAwaiter()
                .GetResult()
                .AsQueryable();

        /// <summary>
        /// Adds the <paramref name="claim"/> given to the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to add the claim to.</param>
        /// <param name="claim">The claim to add to the role.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(role);
            CheckValid(claim);
            return IdentityActors.RoleClaimsCollection().Create(CreateRoleClaim(role, claim));
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
        public override async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(role);
            IdentityResult result = await IdentityActors.RoleCollection().Create(role);
            if (result.Succeeded)
            {
                role?.Copy(await IdentityActors.Role(role.Id).GetRole());
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
        public override Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(role);

            return IdentityActors.RoleCollection().Delete(role.Id, role.ConcurrencyStamp);
        }

        /// <summary>
        /// Finds the role who has the specified ID as an asynchronous operation.
        /// </summary>
        /// <param name="id">The role ID to look for.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>A <see cref="Task"/> that result of the look up.</returns>
        /// <exception cref="ArgumentNullException">id</exception>
        public override async Task<Role> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            var roleId = new Guid(id);
            if (await IdentityActors.RoleCollection().Exist(roleId))
            {
                return await IdentityActors.Role(new Guid(id)).GetRole();
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Finds the role who has the specified normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="normalizedName">The normalized role name to look for.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>A <see cref="Task"/> that result of the look up.</returns>
        /// <exception cref="ArgumentNullException">normalizedName</exception>
        public override async Task<Role> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(normalizedName))
            {
                throw new ArgumentNullException(nameof(normalizedName));
            }
            Guid? roleId = await IdentityActors.RoleCollection().FindByNormalizedName(normalizedName);
            if (roleId != null)
            {
                return await IdentityActors.Role(roleId.Value).GetRole();
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Get the claims associated with the specified <paramref name="role"/> as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose claims should be retrieved.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>A <see cref="Task"/> that contains the claims granted to a role.</returns>
        public override async Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(role);
            return (await IdentityActors.RoleClaims(role.Id).GetAll()).Select(p => new Claim(p.Item1, p.Item2)).ToList();
        }

        /// <summary>
        /// Removes the <paramref name="claim"/> given from the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove from the role.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(role);
            CheckValid(claim);
            return IdentityActors.RoleClaims(role.Id).Remove(claim.Type, claim.Value);
        }

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
        public override async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(role);
            IdentityResult result = await IdentityActors.RoleCollection().Update(role);
            if (result.Succeeded)
            {
                role.Copy(await IdentityActors.Role(role.Id).GetRole());
            }
            return result;
        }

        private void CheckValid(Role? role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (role.Id == default)
            {
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Resources.RoleIdNotDefined));
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
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Resources.ClaimTypeNotDefined));
            }
        }

        private async Task<List<Role>> RoleList()
      => (await Task.WhenAll(
              (await IdentityActors
              .RoleCollection()
              .GetIds())
                  .Select(p => IdentityActors.Role(p).GetRole())))
                  .ToList();
    }
}
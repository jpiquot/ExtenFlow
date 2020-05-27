using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using ExtenFlow.Actors;
using ExtenFlow.Domain.Exceptions;
using ExtenFlow.Identity.Roles.Actors;
using ExtenFlow.Identity.Roles.Application;
using ExtenFlow.Identity.Roles.Commands;
using ExtenFlow.Identity.Roles.Exceptions;
using ExtenFlow.Identity.Roles.Models;
using ExtenFlow.Identity.Roles.Queries;
using ExtenFlow.Infrastructure;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExtenFlow.Identity.Roles.Stores
{
    /// <summary>
    /// The Dapr role store
    /// </summary>
    public sealed class RoleStore : RoleStoreBase<Role, string, UserRole, RoleClaim>, IRoleStore
    {
        private readonly IdentityErrorDescriber _describer;
        private readonly ILogger<RoleStore> _log;
        private readonly IRoleCommandService _roleCommandService;
        private readonly IRoleQueryService _roleQueryService;
        private readonly IUser _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleStore"/> class.
        /// </summary>
        /// <param name="roleCommandService"></param>
        /// <param name="roleQueryService"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="describer">The describer.</param>
        public RoleStore(
            IRoleCommandService roleCommandService,
            IRoleQueryService roleQueryService,
            IUser user,
            ILogger<RoleStore> logger,
            IdentityErrorDescriber? describer = null
            ) : base(describer ?? new IdentityErrorDescriber())
        {
            _roleCommandService = roleCommandService;
            _roleQueryService = roleQueryService;
            _user = user;
            _log = logger;
            _describer = describer ?? new IdentityErrorDescriber();
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public override IQueryable<Role> Roles => GetAllRoles().GetAwaiter().GetResult().AsQueryable();

        /// <summary>
        /// add claim as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="ArgumentNullException">role</exception>
        /// <exception cref="ArgumentNullException">claim</exception>
        /// <exception cref="ArgumentException">role</exception>
        /// <exception cref="ArgumentException">claim</exception>
        /// <exception cref="RoleNotFoundException">Id</exception>
        public override async Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            _ = claim ?? throw new ArgumentNullException(nameof(claim));
            if (role.Id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            if (string.IsNullOrWhiteSpace(claim.Type))
            {
                throw new ArgumentException(Properties.Resources.RoleClaimTypeNotDefined, nameof(claim));
            }
            try
            {
                await _roleCommandService.Tell(new AddRoleClaim(role.Id, claim.Type, claim.Value, role.ConcurrencyStamp, _user.Name));
            }
            catch (EntityNotFoundException e)
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Role.Id), role.Id, e);
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        public override async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            if (role.Id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            try
            {
                await _roleCommandService.Tell(new AddNewRole(role, _user.Name));
            }
            catch (EntityConcurrencyCheckFailedException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.ConcurrencyFailure());
            }
            catch (EntityDuplicateException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.DuplicateRoleName(e.Message));
            }
            catch (InvalidRoleNameException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.InvalidRoleName(e.Message));
            }
            SetRoleValues(role, await _roleQueryService.Ask(new GetRoleDetails(role.Id, _user.Name)));
            return IdentityResult.Success;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        public override async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            if (role.Id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            try
            {
                await _roleCommandService.Tell(new RemoveRole(role.Id, role.ConcurrencyStamp, _user.Name));
            }
            catch (EntityConcurrencyCheckFailedException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;Role&gt;.</returns>
        public override async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(roleId));
            }
            return ToRole(await _roleQueryService.Ask(new GetRoleDetails(roleId, _user.Name)));
        }

        /// <summary>
        /// Finds the by name asynchronous.
        /// </summary>
        /// <param name="normalizedRoleName">Name of the normalized role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;Role&gt;.</returns>
        public override async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(Properties.Resources.InvalidRoleNormalizedName, nameof(normalizedRoleName));
            }
            string? id = await _roleQueryService.Ask(new FindRoleIdByName(normalizedRoleName, _user.Name));
            if (id == null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
            return await FindByIdAsync(id, cancellationToken);
        }

        /// <summary>
        /// Gets the claims asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IList&lt;Claim&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">role</exception>
        /// <exception cref="ArgumentException">role</exception>
        public override Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            if (role.Id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            return _roleQueryService.Ask(new GetRoleClaims(role.Id, _user.Name));
        }

        /// <summary>
        /// Gets the normalized role name asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public override Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            if (role.Id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            return Task.FromResult(role.NormalizedName);
        }

        /// <summary>
        /// Gets the role identifier asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public override Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            if (role.Id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            return Task.FromResult(role.Id);
        }

        /// <summary>
        /// Gets the role name asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public override Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            if (role.Id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            return Task.FromResult(role.Name);
        }

        /// <summary>
        /// remove claim as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="ArgumentNullException">role</exception>
        /// <exception cref="ArgumentNullException">claim</exception>
        /// <exception cref="ArgumentException">role</exception>
        /// <exception cref="ArgumentException">claim</exception>
        /// <exception cref="RoleNotFoundException">Id</exception>
        public override Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            _ = claim ?? throw new ArgumentNullException(nameof(claim));
            if (role.Id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            if (string.IsNullOrWhiteSpace(claim.Type))
            {
                throw new ArgumentException(Properties.Resources.RoleClaimTypeNotDefined, nameof(claim));
            }
            return _roleCommandService.Tell(new RemoveRoleClaim(role.Id, claim.Type, claim.Value, _user.Name, role.ConcurrencyStamp));
        }

        /// <summary>
        /// Sets the normalized role name asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="normalizedName">Name of the normalized.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task.</returns>
        public override async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            IRoleActor actor = _getRoleActor(role.Id);
            if (role.Id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            await actor.Tell(new RenameRole(role.Id, role.Name, normalizedName, role.ConcurrencyStamp, _user.Name));
            SetRoleValues(role, await actor.Ask(new GetRoleDetails(role.Id, _user.Name)));
        }

        /// <summary>
        /// Sets the role name asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task.</returns>
        public override async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            IRoleActor actor = _getRoleActor(role.Id);
            if (role.Id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            await actor.Tell(new RenameRole(role.Id, roleName, role.NormalizedName, role.ConcurrencyStamp, _user.Name));
            SetRoleValues(role, await actor.Ask(new GetRoleDetails(role.Id, _user.Name)));
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        public override async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            try
            {
                await SetRoleNameAsync(role, role.Name, cancellationToken);
                await SetNormalizedRoleNameAsync(role, role.NormalizedName, cancellationToken);
            }
            catch (RoleConcurrencyFailureException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.ConcurrencyFailure());
            }
            catch (DuplicateRoleException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.DuplicateRoleName(e.Message));
            }
            catch (InvalidRoleNameException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.InvalidRoleName(e.Message));
            }
            catch (InvalidRoleNormalizedNameException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.InvalidRoleName(e.Message));
            }
            return IdentityResult.Success;
        }

        private static void SetRoleValues(Role role, RoleDetails details)
        {
            role.Name = details.Name;
            role.NormalizedName = details.NormalizedName;
            role.ConcurrencyStamp = details.ConcurrencyStamp;
        }

        private static Role ToRole(RoleDetails details)
        {
            var role = new Role();
            SetRoleValues(role, details);
            return role;
        }

        private async Task<IList<Role>> GetAllRoles()
        {
            ThrowIfDisposed();
            return await Task.WhenAll((await _collection.All())
                    .Select(p => FindByIdAsync(p, default))
                    .ToList()
               );
        }
    }
}
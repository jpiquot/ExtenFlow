using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using ExtenFlow.Domain.Exceptions;
using ExtenFlow.IdentityServer.Application.Queries;
using ExtenFlow.IdentityServer.Domain.Commands;
using ExtenFlow.IdentityServer.Domain.Exceptions;
using ExtenFlow.IdentityServer.Domain.Models;
using ExtenFlow.Infrastructure;
using ExtenFlow.Messages.Commands;
using ExtenFlow.Messages.Queries;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExtenFlow.IdentityServer.Stores
{
    /// <summary>
    /// The Dapr role store
    /// </summary>
    public sealed class RoleStore : RoleStoreBase<Role, string, UserRole, RoleClaim>, IRoleStore
    {
        private readonly IdentityErrorDescriber _describer;
        private readonly Func<ICommandProcessor> _getCommandProcessor;
        private readonly Func<IQueryRequester> _getQueryRequester;
        private readonly ILogger<RoleStore> _log;
        private readonly IUser _user;
        private ICommandProcessor? _commandProcessor;
        private IQueryRequester? _queryRequester;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleStore"/> class.
        /// </summary>
        /// <param name="getCommandProcessor"></param>
        /// <param name="getQueryRequester"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="describer">The describer.</param>
        public RoleStore(
            Func<ICommandProcessor> getCommandProcessor,
            Func<IQueryRequester> getQueryRequester,
            IUser user,
            ILogger<RoleStore> logger,
            IdentityErrorDescriber? describer = null
            ) : base(describer ?? new IdentityErrorDescriber())
        {
            _getCommandProcessor = getCommandProcessor;
            _getQueryRequester = getQueryRequester;
            _user = user;
            _log = logger;
            _describer = describer ?? new IdentityErrorDescriber();
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public override IQueryable<Role> Roles => GetAllRoles().GetAwaiter().GetResult();

        private ICommandProcessor CommandProcessor => _commandProcessor ?? (_commandProcessor = _getCommandProcessor());

        private IQueryRequester QueryRequester => _queryRequester ?? (_queryRequester = _getQueryRequester());

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
        /// <exception cref="ClientNotFoundException">Id</exception>
        public override async Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            _ = claim ?? throw new ArgumentNullException(nameof(claim));
            if (role.Id == default)
            {
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            if (string.IsNullOrWhiteSpace(claim.Type))
            {
                throw new ArgumentException(Domain.Properties.Resources.RoleClaimTypeNotDefined, nameof(claim));
            }
            try
            {
                await CommandProcessor.Submit(new AddClientClaim(role.Id, claim.Type, claim.Value, role.ConcurrencyStamp, _user.Name), cancellationToken);
            }
            catch (EntityNotFoundException e)
            {
                throw new ClientNotFoundException(CultureInfo.CurrentCulture, nameof(Role.Id), role.Id, e);
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
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            try
            {
                if (string.IsNullOrWhiteSpace(role.ConcurrencyStamp))
                {
                    role.ConcurrencyStamp = Guid.NewGuid().ToBase64String();
                }
                await CommandProcessor.Submit(new AddNewClient(role, _user.Name), cancellationToken);
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
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            try
            {
                await CommandProcessor.Submit(new RemoveClient(role.Id, role.ConcurrencyStamp, _user.Name));
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
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(roleId));
            }
            return ToRole(await QueryRequester.Ask(new GetRoleDetails(roleId, _user.Name)));
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
                throw new ArgumentException(Domain.Properties.Resources.InvalidRoleNormalizedName, nameof(normalizedRoleName));
            }
            string? id = await QueryRequester.Ask(new FindRoleIdByName(normalizedRoleName, _user.Name));
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
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            return QueryRequester.Ask(new GetRoleClaims(role.Id, _user.Name));
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
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(role));
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
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(role));
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
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(role));
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
        public override Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = role ?? throw new ArgumentNullException(nameof(role));
            _ = claim ?? throw new ArgumentNullException(nameof(claim));
            if (role.Id == default)
            {
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            if (string.IsNullOrWhiteSpace(claim.Type))
            {
                throw new ArgumentException(Domain.Properties.Resources.RoleClaimTypeNotDefined, nameof(claim));
            }
            return CommandProcessor.Submit(new RemoveClientClaim(role.Id, claim.Type, claim.Value, _user.Name, role.ConcurrencyStamp));
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
            if (role.Id == default)
            {
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            await CommandProcessor.Submit(new RenameRole(role.Id, role.Name, normalizedName, role.ConcurrencyStamp, _user.Name));
            SetRoleValues(role, await QueryRequester.Ask(new GetRoleDetails(role.Id, _user.Name)));
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
            if (role.Id == default)
            {
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(role));
            }
            await CommandProcessor.Submit(new RenameRole(role.Id, roleName, role.NormalizedName, role.ConcurrencyStamp, _user.Name));
            SetRoleValues(role, await QueryRequester.Ask(new GetRoleDetails(role.Id, _user.Name)));
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
            catch (ClientConcurrencyFailureException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.ConcurrencyFailure());
            }
            catch (DuplicateClientException e)
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

        private static void SetRoleValues(Role role, ClientDetails details)
        {
            role.Name = details.Name;
            role.NormalizedName = details.NormalizedName;
            role.ConcurrencyStamp = details.ConcurrencyStamp;
        }

        private static Role ToRole(ClientDetails details)
        {
            var role = new Role();
            SetRoleValues(role, details);
            return role;
        }

        private async Task<IQueryable<Role>> GetAllRoles()
                                                                                                                                                    => (await Task.WhenAll
                (
                    (await QueryRequester.Ask(new GetRoleIds(_user.Name)))
                        .Select(p => QueryRequester.Ask(new GetRoleDetails(p, _user.Name)))
                        .ToList())
                )
                .Select(p => ToRole(p))
                .AsQueryable();
    }
}
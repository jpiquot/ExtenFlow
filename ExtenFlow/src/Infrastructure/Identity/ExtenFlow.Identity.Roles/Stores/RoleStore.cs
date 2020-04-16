using System;
using System.Threading;
using System.Threading.Tasks;

using ExtenFlow.Actors;
using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Roles.Stores;
using ExtenFlow.Infrastructure;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Roles
{
    /// <summary>
    /// The Dapr role store
    /// </summary>
    public sealed class ActorRoleStore : IRoleStore
    {
        private readonly IdentityErrorDescriber _describer;
        private readonly Func<IUniqueIndexActor> _getNameIndex;
        private readonly Func<IUniqueIndexActor> _getNormaliedNameIndex;
        private readonly IUser _user;

        private Func<Guid, IRoleActor> _getRoleActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorRoleStore"/> class.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="getRoleActor"></param>
        /// <param name="getNameIndex"></param>
        /// <param name="getNormaliedNameIndex"></param>
        /// <param name="describer">The describer.</param>
        public ActorRoleStore(
            IUser user,
            Func<Guid, IRoleActor> getRoleActor,
            Func<IUniqueIndexActor> getNameIndex,
            Func<IUniqueIndexActor> getNormaliedNameIndex,
            IdentityErrorDescriber? describer = null)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _getRoleActor = getRoleActor;
            _getNameIndex = getNameIndex;
            _getNormaliedNameIndex = getNormaliedNameIndex;
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
        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            _ = role ?? throw new ArgumentNullException(nameof(role));
            IRoleActor actor = _getRoleActor(role.Id);
            try
            {
                await actor.Tell(new CreateNewRole(role, _user.Name));
            }
            catch (RoleConcurrencyFailureException)
            {
                return IdentityResult.Failed(_describer.ConcurrencyFailure());
            }
            catch (DuplicateRoleException e)
            {
                return IdentityResult.Failed(_describer.DuplicateRoleName(e.Message));
            }
            catch (InvalidRoleNameException e)
            {
                return IdentityResult.Failed(_describer.InvalidRoleName(e.Message));
            }
            RoleDetailsViewModel details = await actor.Ask(new GetRoleDetails(role.Id.ToString(), _user.Name));
            SetRoleValues(role, details);
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
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            _ = role ?? throw new ArgumentNullException(nameof(role));
            IRoleActor actor = _getRoleActor(role.Id);
            try
            {
                await actor.Tell(new DeleteRole(role.Id.ToString(), role.ConcurrencyStamp, _user.Name));
            }
            catch (RoleConcurrencyFailureException)
            {
                return IdentityResult.Failed(_describer.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

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
        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(roleId));
            }
            IRoleActor actor = _getRoleActor(new Guid(roleId));
            return ToRole(await actor.Ask(new GetRoleDetails(roleId, _user.Name)));
        }

        /// <summary>
        /// Finds the by name asynchronous.
        /// </summary>
        /// <param name="normalizedRoleName">Name of the normalized role.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;Role&gt;.</returns>
        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException(Properties.Resources.InvalidRoleNormalizedName, nameof(normalizedRoleName));
            }
            IUniqueIndexActor index = _getNormaliedNameIndex();
            var id = await index.GetIdentifier(normalizedRoleName);
            if (id == null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
            return await FindByIdAsync(id, cancellationToken);
        }

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

        private static void SetRoleValues(Role role, RoleDetailsViewModel details)
        {
            role.Name = details.Name;
            role.NormalizedName = details.NormalizedName;
            role.ConcurrencyStamp = details.ConcurrencyStamp;
        }

        private static Role ToRole(RoleDetailsViewModel details)
        {
            var role = new Role();
            SetRoleValues(role, details);
            return role;
        }
    }
}
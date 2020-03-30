// Copyright (c) .NET Foundation. All rights reserved. Licensed under the Apache License, Version
// 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using ExtenFlow.Identity.DaprActorsStore;
using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Dapr
{
    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>

    public class UserStore :
        UserStoreBase<User, Role, Guid, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>,
        IProtectedUserStore<User>
    {
        /// <summary>
        /// Creates a new instance of the store.
        /// </summary>
        public UserStore() : base(new IdentityErrorDescriber())
        {
        }

        /// <summary>
        /// List of users as Queryable
        /// </summary>
        /// <remarks>Warning : a call to this object reads all the users in the database.</remarks>
        public override IQueryable<User> Users => UserList().GetAwaiter().GetResult().AsQueryable();

        private async Task<List<User>> UserList()
            => (await Task.WhenAll((await GetUserCollectionActor().GetIds())
                .Select(p => GetUserActor(p).GetUser())))
                .ToList();

        private static IUserActor GetUserActor(Guid userId) => ActorProxy.Create<IUserActor>(new ActorId(userId.ToString()), nameof(UserActor));

        private static IRoleActor GetRoleActor(Guid roleId) => ActorProxy.Create<IRoleActor>(new ActorId(roleId.ToString()), nameof(RoleActor));

        private static IUserLoginsActor GetUserLoginsActor(Guid userId) => ActorProxy.Create<IUserLoginsActor>(new ActorId(userId.ToString()), nameof(UserLoginsActor));

        private static IUserLoginsCollectionActor GetUserLoginsCollectionActor() => ActorProxy.Create<IUserLoginsCollectionActor>(new ActorId(nameof(IUserLoginsCollectionActor)), nameof(UserLoginsCollectionActor));

        private static IUserRoleCollectionActor GetUserRoleCollectionActor() => ActorProxy.Create<IUserRoleCollectionActor>(new ActorId(nameof(IUserRoleCollectionActor)), nameof(UserRoleCollectionActor));

        private static IUserClaimsActor GetUserClaimsActor(Guid userId) => ActorProxy.Create<IUserClaimsActor>(new ActorId(userId.ToString()), nameof(UserClaimsActor));

        private static IUserClaimsCollectionActor GetUserClaimsCollectionActor() => ActorProxy.Create<IUserClaimsCollectionActor>(new ActorId(nameof(IUserClaimsCollectionActor)), nameof(UserClaimsCollectionActor));

        private static IUserTokensActor GetUserTokensActor(Guid userId) => ActorProxy.Create<IUserTokensActor>(new ActorId(userId.ToString()), nameof(UserTokensActor));

        private static IUserCollectionActor GetUserCollectionActor() => ActorProxy.Create<IUserCollectionActor>(new ActorId(nameof(UserCollectionActor)), nameof(UserCollectionActor));

        private static IRoleCollectionActor GetRoleCollectionActor() => ActorProxy.Create<IRoleCollectionActor>(new ActorId(nameof(RoleCollectionActor)), nameof(RoleCollectionActor));

        private void CheckValid(User? user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.Id == default)
            {
                throw new ArgumentNullException(nameof(User) + "." + nameof(User.Id));
            }
        }

        /// <summary>
        /// Creates the specified <paramref name="user"/> in the user store.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see
        /// cref="IdentityResult"/> of the creation operation.
        /// </returns>
        public async override Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(user);
            IdentityResult result = await GetUserCollectionActor().Create(user);
            if (result.Succeeded)
            {
                user.Copy(await GetUserActor(user.Id).GetUser());
            }
            return result;
        }

        /// <summary>
        /// Updates the specified <paramref name="user"/> in the user store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see
        /// cref="IdentityResult"/> of the update operation.
        /// </returns>
        public override async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(user);
            IdentityResult result = await GetUserCollectionActor().Update(user);
            if (result.Succeeded)
            {
                user.Copy(await GetUserActor(user.Id).GetUser());
            }
            return result;
        }

        /// <summary>
        /// Deletes the specified <paramref name="user"/> from the user store.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see
        /// cref="IdentityResult"/> of the update operation.
        /// </returns>
        public override Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            CheckValid(user);

            return GetUserCollectionActor().Delete(user.Id, user.ConcurrencyStamp);
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user
        /// matching the specified <paramref name="userId"/> if it exists.
        /// </returns>
        public override async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Guid id = ConvertIdFromString(userId);
            if (!await GetUserCollectionActor().Exist(id))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
            return await GetUserActor(id).GetUser();
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified normalized user name.
        /// </summary>
        /// <param name="normalizedUserName">The normalized user name to search for.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user
        /// matching the specified <paramref name="normalizedUserName"/> if it exists.
        /// </returns>
        public override async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Guid? id = await GetUserCollectionActor().FindByNormalizedName(normalizedUserName);
            if (id != null)
            {
                return await GetUserActor(id.Value).GetUser();
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Return a role with the normalized name if it exists.
        /// </summary>
        /// <param name="normalizedRoleName">The normalized role name.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The role if it exists.</returns>
        protected override async Task<Role> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            Guid? id = await GetRoleCollectionActor().FindByNormalizedName(normalizedRoleName);
            if (id != null)
            {
                return await GetRoleActor(id.Value).GetRole();
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Return a user role for the userId and roleId if it exists.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="roleId">The role's id.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The user role if it exists.</returns>
        protected override async Task<UserRole> FindUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userId == default || roleId == default || !await GetUserCollectionActor().Exist(userId) || !await GetRoleCollectionActor().Exist(roleId))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
            }
            return (await GetUserRoleCollectionActor().Exist(userId, roleId)) ? new UserRole() { UserId = userId, RoleId = roleId } : null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Return a user with the matching userId if it exists.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The user if it exists.</returns>
        protected override async Task<User> FindUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userId == default)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (await GetUserCollectionActor().Exist(userId))
            {
                return await GetUserActor(userId).GetUser();
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Return a user login with the matching userId, provider, providerKey if it exists.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="loginProvider">The login provider name.</param>
        /// <param name="providerKey">
        /// The key provided by the <paramref name="loginProvider"/> to identify a user.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The user login if it exists.</returns>
        protected override async Task<UserLogin> FindUserLoginAsync(Guid userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (userId == default)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            IUserLoginsActor actor = GetUserLoginsActor(userId);
            UserLoginInfo? userLogin = await actor.Find(loginProvider, providerKey);
            if (userLogin == null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
            return new UserLogin() { UserId = userId, LoginProvider = userLogin.LoginProvider, ProviderDisplayName = userLogin.ProviderDisplayName, ProviderKey = userLogin.ProviderKey };
        }

        /// <summary>
        /// Return a user login with provider, providerKey if it exists.
        /// </summary>
        /// <param name="loginProvider">The login provider name.</param>
        /// <param name="providerKey">
        /// The key provided by the <paramref name="loginProvider"/> to identify a user.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The user login if it exists.</returns>
        protected override Task<UserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Task.FromException<UserLogin>(new ArgumentNullException(nameof(loginProvider)));
            }
            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return Task.FromException<UserLogin>(new ArgumentNullException(nameof(providerKey)));
            }
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return GetUserLoginsCollectionActor().FindByProvider(loginProvider, providerKey);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }

        /// <summary>
        /// Adds the given <paramref name="normalizedRoleName"/> to the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to add the role to.</param>
        /// <param name="normalizedRoleName">The role to add.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override async Task AddToRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }
            Guid? roleId = await GetRoleCollectionActor().FindByNormalizedName(normalizedRoleName);
            if (roleId == null)
            {
                throw new KeyNotFoundException($"The role '{normalizedRoleName}' was not found.");
            }
            await GetUserRoleCollectionActor().Create(user.Id, roleId.Value);
        }

        /// <summary>
        /// Removes the given <paramref name="normalizedRoleName"/> from the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to remove the role from.</param>
        /// <param name="normalizedRoleName">The role to remove.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async override Task RemoveFromRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }
            Guid? roleId = await GetRoleCollectionActor().FindByNormalizedName(normalizedRoleName);
            if (roleId == null)
            {
                throw new KeyNotFoundException($"The role '{normalizedRoleName}' was not found.");
            }
            await GetUserRoleCollectionActor().Delete(user.Id, roleId.Value);
        }

        /// <summary>
        /// Retrieves the roles the specified <paramref name="user"/> is a member of.
        /// </summary>
        /// <param name="user">The user whose roles should be retrieved.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that contains the roles the user is a member of.
        /// </returns>
        public override async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return (await GetUserRoleCollectionActor().GetRoleIds(user.Id)).Select(p => p.ToString()).ToList();
        }

        /// <summary>
        /// Returns a flag indicating if the specified user is a member of the give <paramref name="normalizedRoleName"/>.
        /// </summary>
        /// <param name="user">The user whose role membership should be checked.</param>
        /// <param name="normalizedRoleName">The role to check membership of</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> containing a flag indicating if the specified user is a
        /// member of the given group. If the user is a member of the group the returned value with
        /// be true, otherwise it will be false.
        /// </returns>
        public override async Task<bool> IsInRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }
            Guid? roleId = await GetRoleCollectionActor().FindByNormalizedName(normalizedRoleName);
            if (roleId != null)
            {
                UserRole userRole = await FindUserRoleAsync(user.Id, roleId.Value, cancellationToken);
                return userRole != null;
            }
            return false;
        }

        /// <summary>
        /// Get the claims associated with the specified <paramref name="user"/> as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user whose claims should be retrieved.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a user.</returns>
        public async override Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return (await GetUserClaimsActor(user.Id).GetAll()).Select(p => new Claim(p.Item1, p.Item2)).ToList();
        }

        /// <summary>
        /// Adds the <paramref name="claims"/> given to the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to add the claim to.</param>
        /// <param name="claims">The claim to add to the user.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            IUserClaimsCollectionActor actor = GetUserClaimsCollectionActor();
            foreach (Claim claim in claims)
            {
                await actor.Create(CreateUserClaim(user, claim));
            }
        }

        /// <summary>
        /// Replaces the <paramref name="claim"/> on the specified <paramref name="user"/>, with the
        /// <paramref name="newClaim"/>.
        /// </summary>
        /// <param name="user">The user to replace the claim on.</param>
        /// <param name="claim">The claim replace.</param>
        /// <param name="newClaim">The new claim replacing the <paramref name="claim"/>.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async override Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            if (newClaim == null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }
            IUserClaimsCollectionActor actor = GetUserClaimsCollectionActor();
            await actor.Delete(CreateUserClaim(user, claim));
            await actor.Create(CreateUserClaim(user, newClaim));
        }

        /// <summary>
        /// Removes the <paramref name="claims"/> given from the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to remove the claims from.</param>
        /// <param name="claims">The claim to remove.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async override Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            IUserClaimsCollectionActor actor = GetUserClaimsCollectionActor();
            foreach (Claim claim in claims)
            {
                await actor.Delete(CreateUserClaim(user, claim));
            }
        }

        /// <summary>
        /// Adds the <paramref name="login"/> given to the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to add the login to.</param>
        /// <param name="login">The login to add to the user.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override Task AddLoginAsync(User user, UserLoginInfo login,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                return Task.FromException(new ArgumentNullException(nameof(user)));
            }
            if (login == null)
            {
                return Task.FromException(new ArgumentNullException(nameof(login)));
            }
            return GetUserLoginsCollectionActor().Create(CreateUserLogin(user, login));
        }

        /// <summary>
        /// Removes the <paramref name="loginProvider"/> given from the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to remove the login from.</param>
        /// <param name="loginProvider">The login to remove from the user.</param>
        /// <param name="providerKey">
        /// The key provided by the <paramref name="loginProvider"/> to identify a user.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                return Task.FromException(new ArgumentOutOfRangeException(Resource.UserIdNotDefined));
            }
            return GetUserLoginsCollectionActor().Delete(user.Id, loginProvider, providerKey);
        }

        /// <summary>
        /// Retrieves the associated logins for the specified <param ref="user"/>.
        /// </summary>
        /// <param name="user">The user whose associated logins to retrieve.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see
        /// cref="UserLoginInfo"/> for the specified <paramref name="user"/>, if any.
        /// </returns>
        public override Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                return Task.FromException<IList<UserLoginInfo>>(new ArgumentOutOfRangeException(Resource.UserIdNotDefined));
            }
            return GetUserLoginsActor(user.Id).GetAll();
        }

        /// <summary>
        /// Retrieves the user associated with the specified login provider and login provider key.
        /// </summary>
        /// <param name="loginProvider">The login provider who provided the <paramref name="providerKey"/>.</param>
        /// <param name="providerKey">
        /// The key provided by the <paramref name="loginProvider"/> to identify a user.
        /// </param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing the user, if any which
        /// matched the specified login provider and key.
        /// </returns>
        public async override Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            UserLogin userLogin = await FindUserLoginAsync(loginProvider, providerKey, cancellationToken);
            if (userLogin != null)
            {
                return await FindUserAsync(userLogin.UserId, cancellationToken);
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Gets the user, if any, associated with the specified, normalized email address.
        /// </summary>
        /// <param name="normalizedEmail">The normalized email address to return the user for.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The task object containing the results of the asynchronous lookup operation, the user if
        /// any associated with the specified normalized email address.
        /// </returns>
        public override Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return Task.FromResult(Users.Where(u => u.NormalizedEmail == normalizedEmail).SingleOrDefault());
        }

        /// <summary>
        /// Retrieves all users with the specified claim.
        /// </summary>
        /// <param name="claim">The claim whose users should be retrieved.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> contains a list of users, if any, that contain the specified claim.
        /// </returns>
        public override Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (claim == null || string.IsNullOrWhiteSpace(claim.Type))
            {
                throw new ArgumentOutOfRangeException(Resource.UserIdNotDefined);
            }
            return GetUserClaimsCollectionActor().GetUsers(claim.Type, claim.Value);
        }

        /// <summary>
        /// Retrieves all users in the specified role.
        /// </summary>
        /// <param name="normalizedRoleName">The role whose users should be retrieved.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> contains a list of users, if any, that are in the specified role.
        /// </returns>
        public async override Task<IList<User>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(normalizedRoleName))
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }
            Guid? roleId = await GetRoleCollectionActor().FindByNormalizedName(normalizedRoleName);
            if (roleId == null)
            {
                throw new KeyNotFoundException($"Role with normalized name '{normalizedRoleName}' not found.");
            }
            return (await Task.WhenAll(
                (await GetUserRoleCollectionActor().GetUserIds(roleId.Value))
                .Select(p => GetUserActor(p).GetUser()))
                )
                .ToList();
        }

        /// <summary>
        /// Find a user token if it exists.
        /// </summary>
        /// <param name="user">The token owner.</param>
        /// <param name="loginProvider">The login provider for the token.</param>
        /// <param name="name">The name of the token.</param>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation
        /// should be canceled.
        /// </param>
        /// <returns>The user token if it exists.</returns>
        protected override async Task<UserToken> FindTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (user == null || user.Id == default)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                throw new ArgumentNullException(nameof(loginProvider));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            string? value = await GetUserTokensActor(user.Id).FindValue(loginProvider, name);
            if (value == null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
            return new UserToken() { UserId = user.Id, LoginProvider = loginProvider, Name = name, Value = value };
        }

        /// <summary>
        /// Add a new user token.
        /// </summary>
        /// <param name="token">The token to be added.</param>
        /// <returns></returns>
        protected override Task AddUserTokenAsync(UserToken token)
        {
            ThrowIfDisposed();
            if (token == null || token.UserId == default)
            {
                return Task.FromException<IList<UserLoginInfo>>(new ArgumentOutOfRangeException(Resource.UserIdNotDefined));
            }
            return GetUserTokensActor(token.UserId).Add(token.LoginProvider, token.Name, token.Value);
        }

        /// <summary>
        /// Remove a new user token.
        /// </summary>
        /// <param name="token">The token to be removed.</param>
        /// <returns></returns>
        protected override Task RemoveUserTokenAsync(UserToken token)
        {
            ThrowIfDisposed();
            if (token == null || token.UserId == default)
            {
                return Task.FromException<IList<UserLoginInfo>>(new ArgumentOutOfRangeException(Resource.UserIdNotDefined));
            }
            return GetUserTokensActor(token.UserId).Remove(token.LoginProvider, token.Name);
        }
    }
}
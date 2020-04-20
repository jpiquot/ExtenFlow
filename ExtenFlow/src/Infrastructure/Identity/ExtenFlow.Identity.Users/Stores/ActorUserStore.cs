using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using ExtenFlow.Actors;
using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Users.Actors;
using ExtenFlow.Identity.Users.Commands;
using ExtenFlow.Identity.Users.Exceptions;
using ExtenFlow.Identity.Users.Queries;
using ExtenFlow.Infrastructure;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExtenFlow.Identity.Users.Stores
{
    /// <summary>
    /// The Dapr user store
    /// </summary>
    public sealed class ActorUserStore : IUserStore
    {
        private readonly ICollectionActor _collection;
        private readonly IdentityErrorDescriber _describer;
        private readonly Func<Guid, IUserActor> _getUserActor;
        private readonly Func<Guid, IUserClaimsActor> _getUserClaimsActor;
        private readonly ILogger<ActorUserStore> _log;
        private readonly IUniqueIndexActor _normaliedNameIndex;
        private readonly IUser _user;

        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorUserStore"/> class.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="getUserActor"></param>
        /// <param name="collection"></param>
        /// <param name="normalizedNameIndex"></param>
        /// <param name="getUserClaimsActor"></param>
        /// <param name="logger"></param>
        /// <param name="describer">The describer.</param>
        public ActorUserStore(
            IUser user,
            Func<Guid, IUserActor> getUserActor,
            ICollectionActor collection,
            IUniqueIndexActor normalizedNameIndex,
            Func<Guid, IUserClaimsActor> getUserClaimsActor,
            ILogger<ActorUserStore> logger,
            IdentityErrorDescriber? describer = null
            )
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _getUserActor = getUserActor;
            _collection = collection;
            _normaliedNameIndex = normalizedNameIndex;
            _getUserClaimsActor = getUserClaimsActor;
            _log = logger;
            _describer = describer ?? new IdentityErrorDescriber();
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>The users.</value>
        public IQueryable<User> Users => GetAllUsers().GetAwaiter().GetResult().AsQueryable();

        /// <summary>
        /// add claims as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="claims">The claims.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.ArgumentNullException">claims</exception>
        /// <exception cref="System.ArgumentException">user</exception>
        /// <exception cref="System.ArgumentException">claims</exception>
        /// <exception cref="ExtenFlow.Identity.Users.Exceptions.UserNotFoundException">Id</exception>
        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            _ = claims ?? throw new ArgumentNullException(nameof(claims));
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            if (claims.Any(p => string.IsNullOrWhiteSpace(p.Type)))
            {
                throw new ArgumentException(Properties.Resources.UserClaimTypeNotDefined, nameof(claims));
            }
            IUserActor userActor = _getUserActor(user.Id);
            if (!await userActor.IsInitialized())
            {
                throw new UserNotFoundException(CultureInfo.CurrentCulture, nameof(User.Id), user.Id.ToString());
            }
            IUserClaimsActor claimActor = _getUserClaimsActor(user.Id);

            await claimActor.Tell(new AddUserClaims(user.Id.ToString(), claims.ToDictionary(p => p.Type, t => t.Value), _user.Name));
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            IUserActor actor = _getUserActor(user.Id);
            try
            {
                await actor.Tell(new RegisterNewUser(user, _user.Name));
            }
            catch (UserConcurrencyFailureException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.ConcurrencyFailure());
            }
            catch (DuplicateUserException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.DuplicateUserName(e.Message));
            }
            catch (InvalidUserNameException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.InvalidUserName(e.Message));
            }
            UserDetailsModel details = await actor.Ask(new GetUserDetails(user.Id.ToString(), _user.Name));
            SetUserValues(user, details);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            IUserActor actor = _getUserActor(user.Id);
            try
            {
                await actor.Tell(new UnregisterUser(user.Id.ToString(), user.ConcurrencyStamp, _user.Name));
            }
            catch (UserConcurrencyFailureException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose() => _disposed = true;

        /// <summary>
        /// Finds the by identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;User&gt;.</returns>
        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(userId));
            }
            IUserActor actor = _getUserActor(new Guid(userId));
            return ToUser(await actor.Ask(new GetUserDetails(userId, _user.Name)));
        }

        /// <summary>
        /// Finds the by name asynchronous.
        /// </summary>
        /// <param name="normalizedUserName">Name of the normalized user.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;User&gt;.</returns>
        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(normalizedUserName))
            {
                throw new ArgumentException(Properties.Resources.InvalidUserNormalizedName, nameof(normalizedUserName));
            }
            string? id = await _normaliedNameIndex.GetIdentifier(normalizedUserName);
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
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IList&lt;Claim&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        /// <exception cref="ArgumentException">user</exception>
        public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            IUserClaimsActor actor = _getUserClaimsActor(user.Id);
            return actor.Ask<IList<Claim>>(new GetUserClaims(user.Id.ToString(), _user.Name));
        }

        /// <summary>
        /// Gets the normalized user name asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            return Task.FromResult(user.NormalizedUserName);
        }

        /// <summary>
        /// Gets the user identifier asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            return Task.FromResult(user.Id.ToString());
        }

        /// <summary>
        /// Gets the user name asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        /// <summary>
        /// get users for claim as an asynchronous operation.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>IList&lt;User&gt;.</returns>
        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var list = (await _collection.All())
                .Select(p => new
                {
                    Id = p,
                    Exist = _getUserClaimsActor(new Guid(p))
                                .Exist(claim.Type, claim.Value)
                })
                .ToList();
            var result = await Task.WhenAll(list.Select(p => p.Exist).ToList());
            var details = await Task.WhenAll(list
                .Where(p => p.Exist.Result)
                .Select(p => _getUserActor(new Guid(p.Id)).Ask(new GetUserDetails(p.Id, _user.Name))));
            return details.Select(p => new User
            {
                Id = p.Id,
                NormalizedUserName = p.NormalizedName,
                UserName = p.Name,
                ConcurrencyStamp = p.ConcurrencyStamp
            }).ToList();
        }

        /// <summary>
        /// remove claims as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="claims">The claims.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.ArgumentNullException">claims</exception>
        /// <exception cref="System.ArgumentException">user</exception>
        /// <exception cref="System.ArgumentException">claims</exception>
        /// <exception cref="ExtenFlow.Identity.Users.Exceptions.UserNotFoundException">Id</exception>
        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            _ = claims ?? throw new ArgumentNullException(nameof(claims));
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            if (claims.Any(p => string.IsNullOrWhiteSpace(p.Type)))
            {
                throw new ArgumentException(Properties.Resources.UserClaimTypeNotDefined, nameof(claims));
            }
            IUserActor userActor = _getUserActor(user.Id);
            if (!await userActor.IsInitialized())
            {
                throw new UserNotFoundException(CultureInfo.CurrentCulture, nameof(User.Id), user.Id.ToString());
            }
            IUserClaimsActor claimActor = _getUserClaimsActor(user.Id);

            await claimActor.Tell(new RemoveUserClaims(user.Id.ToString(), claims.ToDictionary(p => p.Type, p => p.Value), _user.Name));
        }

        /// <summary>
        /// replace claim as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="claim">The claim.</param>
        /// <param name="newClaim">The new claim.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.ArgumentNullException">claim</exception>
        /// <exception cref="System.ArgumentNullException">newClaim</exception>
        /// <exception cref="System.ArgumentException">user</exception>
        /// <exception cref="System.ArgumentException">claim</exception>
        /// <exception cref="System.ArgumentException">newClaim</exception>
        /// <exception cref="ExtenFlow.Identity.Users.Exceptions.UserNotFoundException">Id</exception>
        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            _ = claim ?? throw new ArgumentNullException(nameof(claim));
            _ = newClaim ?? throw new ArgumentNullException(nameof(newClaim));
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            if (string.IsNullOrWhiteSpace(claim.Type))
            {
                throw new ArgumentException(Properties.Resources.UserClaimTypeNotDefined, nameof(claim));
            }
            if (string.IsNullOrWhiteSpace(newClaim.Type))
            {
                throw new ArgumentException(Properties.Resources.UserClaimTypeNotDefined, nameof(newClaim));
            }
            IUserActor userActor = _getUserActor(user.Id);
            if (!await userActor.IsInitialized())
            {
                throw new UserNotFoundException(CultureInfo.CurrentCulture, nameof(User.Id), user.Id.ToString());
            }
            IUserClaimsActor claimActor = _getUserClaimsActor(user.Id);

            await claimActor.Tell(new RemoveUserClaims(user.Id.ToString(), new Dictionary<string, string> { { claim.Type, claim.Value } }, _user.Name));
            await claimActor.Tell(new AddUserClaims(user.Id.ToString(), new Dictionary<string, string> { { newClaim.Type, newClaim.Value } }, _user.Name));
        }

        /// <summary>
        /// Sets the normalized user name asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="normalizedName">Name of the normalized.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task.</returns>
        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            IUserActor actor = _getUserActor(user.Id);
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            await actor.Tell(new RenameUser(user.Id.ToString(), user.UserName, normalizedName, user.ConcurrencyStamp, _user.Name));
            SetUserValues(user, await actor.Ask(new GetUserDetails(user.Id.ToString(), _user.Name)));
        }

        /// <summary>
        /// Sets the user name asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task.</returns>
        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            IUserActor actor = _getUserActor(user.Id);
            if (user.Id == default)
            {
                throw new ArgumentException(Properties.Resources.UserIdNotDefined, nameof(user));
            }
            await actor.Tell(new RenameUser(user.Id.ToString(), userName, user.NormalizedUserName, user.ConcurrencyStamp, _user.Name));
            SetUserValues(user, await actor.Ask(new GetUserDetails(user.Id.ToString(), _user.Name)));
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            _ = user ?? throw new ArgumentNullException(nameof(user));
            try
            {
                await SetUserNameAsync(user, user.UserName, cancellationToken);
                await SetNormalizedUserNameAsync(user, user.NormalizedUserName, cancellationToken);
            }
            catch (UserConcurrencyFailureException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.ConcurrencyFailure());
            }
            catch (DuplicateUserException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.DuplicateUserName(e.Message));
            }
            catch (InvalidUserNameException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.InvalidUserName(e.Message));
            }
            catch (InvalidUserNormalizedNameException e)
            {
                _log.LogWarning(e.Message);
                return IdentityResult.Failed(_describer.InvalidUserName(e.Message));
            }
            return IdentityResult.Success;
        }

        private static void SetUserValues(User user, UserDetailsModel details)
        {
            user.UserName = details.Name;
            user.NormalizedUserName = details.NormalizedName;
            user.ConcurrencyStamp = details.ConcurrencyStamp;
        }

        private static User ToUser(UserDetailsModel details)
        {
            var user = new User();
            SetUserValues(user, details);
            return user;
        }

        private async Task<IList<User>> GetAllUsers()
        {
            ThrowIfDisposed();
            return await Task.WhenAll((await _collection.All())
                    .Select(p => FindByIdAsync(p, default))
                    .ToList()
               );
        }

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
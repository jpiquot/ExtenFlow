using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The User Login Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IRoleActor"/>
    public class UserLoginsActor : BaseActor<HashSet<UserLoginInfo>>, IUserLoginsActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginsActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserLoginsActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Gets the user login.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>The user login info object.</returns>
        public Task<UserLoginInfo> Get(string loginProvider, string providerKey)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Task.FromException<UserLoginInfo>(new ArgumentNullException(nameof(loginProvider)));
            }
            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return Task.FromException<UserLoginInfo>(new ArgumentNullException(nameof(providerKey)));
            }
            UserLoginInfo? login = State.Where(p => p.LoginProvider == loginProvider && p.ProviderKey == providerKey).FirstOrDefault();
            if (login == null)
            {
                return Task.FromException<UserLoginInfo>(new KeyNotFoundException($"User Id = '{Id.GetId()}'; Login Provider = '{nameof(providerKey)}'; Povider Key = '{providerKey}'."));
            }
            return Task.FromResult(login);
        }

        /// <summary>
        /// Finds the user login.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>The user login info object if exists, else null.</returns>
        public Task<UserLoginInfo?> Find(string loginProvider, string providerKey)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Task.FromException<UserLoginInfo?>(new ArgumentNullException(nameof(loginProvider)));
            }
            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return Task.FromException<UserLoginInfo?>(new ArgumentNullException(nameof(providerKey)));
            }
            return Task.FromResult<UserLoginInfo?>(State.Where(p => p.LoginProvider == loginProvider && p.ProviderKey == providerKey).FirstOrDefault());
        }

        /// <summary>
        /// Gets all logins of this user.
        /// </summary>
        /// <returns>The list of logins.</returns>
        public Task<IList<UserLoginInfo>> GetAll() => Task.FromResult<IList<UserLoginInfo>>(State.ToList());

        /// <summary>
        /// Adds the specified user login information.
        /// </summary>
        /// <param name="userLoginInfo">The user login information.</param>
        /// <returns></returns>
        public Task Add(UserLoginInfo userLoginInfo)
        {
            if (userLoginInfo == null)
            {
                return Task.FromException(new ArgumentNullException(nameof(userLoginInfo)));
            }
            if (string.IsNullOrWhiteSpace(userLoginInfo.LoginProvider))
            {
                return Task.FromException(new ArgumentOutOfRangeException(Resource.LoginProviderNotDefined));
            }
            if (string.IsNullOrWhiteSpace(userLoginInfo.ProviderKey))
            {
                return Task.FromException(new ArgumentOutOfRangeException(Resource.ProviderKeyNotDefined));
            }
            if (State.Any(p => p.LoginProvider == userLoginInfo.LoginProvider && p.ProviderKey == userLoginInfo.ProviderKey))
            {
                return Task.FromException(new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resource.DuplicateUserLogin, Id.GetId(), userLoginInfo.LoginProvider, userLoginInfo.ProviderKey)));
            }
            State.Add(userLoginInfo);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes the specified login provider.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns></returns>
        public Task Delete(string loginProvider, string providerKey)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Task.FromException(new ArgumentNullException(nameof(loginProvider)));
            }
            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return Task.FromException(new ArgumentNullException(nameof(providerKey)));
            }
            UserLoginInfo? userLoginInfo = State.FirstOrDefault(p => p.LoginProvider == loginProvider && p.ProviderKey == providerKey);
            if (userLoginInfo == null)
            {
                return Task.FromException(new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resource.UserLoginNotFound, Id.GetId(), loginProvider, providerKey)));
            }
            State.Remove(userLoginInfo);
            return Task.CompletedTask;
        }
    }
}
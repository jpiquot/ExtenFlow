using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Properties;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The user collection actor class
    /// </summary>
    public class UserLoginsCollectionActor : ActorBase<UserLoginsCollectionState>, IUserLoginsCollectionActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginsCollectionActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserLoginsCollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Creates the specified user login.
        /// </summary>
        /// <param name="userLogin">The user login.</param>
        /// <exception cref="ArgumentNullException">userLogin</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task Create(UserLogin userLogin)
        {
            if (userLogin == null)
            {
                throw new ArgumentNullException(nameof(userLogin));
            }
            if (userLogin.UserId == default)
            {
                throw new ArgumentOutOfRangeException(Resources.UserIdNotDefined);
            }
            if (string.IsNullOrWhiteSpace(userLogin.LoginProvider))
            {
                throw new ArgumentOutOfRangeException(Resources.LoginProviderNotDefined);
            }
            if (string.IsNullOrWhiteSpace(userLogin.ProviderKey))
            {
                throw new ArgumentOutOfRangeException(Resources.ProviderKeyNotDefined);
            }
            await IdentityActors.UserLogins(userLogin.UserId).AddLogin(new UserLoginInfo(userLogin.LoginProvider, userLogin.ProviderKey, userLogin.ProviderDisplayName));
            State.Add(userLogin.LoginProvider, userLogin.ProviderKey, userLogin.UserId);
            await SetStateData();
        }

        /// <summary>
        /// Delete the user login
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task Delete(Guid userId, string loginProvider, string providerKey)
        {
            if (userId == default)
            {
                throw new ArgumentOutOfRangeException(Resources.UserIdNotDefined);
            }
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                throw new ArgumentOutOfRangeException(Resources.LoginProviderNotDefined);
            }
            if (string.IsNullOrWhiteSpace(providerKey))
            {
                throw new ArgumentOutOfRangeException(Resources.ProviderKeyNotDefined);
            }
            await IdentityActors.UserLogins(userId).DeleteLogin(loginProvider, providerKey);
            State.Remove(loginProvider, providerKey, userId);
            await SetStateData();
        }

        /// <summary>
        /// Finds the user login by provider.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>The user login object if found, else null.</returns>
        public async Task<UserLogin?> FindByProvider(string loginProvider, string providerKey)
        {
            Guid? userId = State.GetProviderKeys(loginProvider)[providerKey];
            if (userId != null)
            {
                UserLoginInfo? loginInfo = await IdentityActors.UserLogins(userId.Value).FindLogin(loginProvider, providerKey);
                if (loginInfo != null)
                {
                    return new UserLogin() { LoginProvider = loginInfo.LoginProvider, ProviderKey = loginInfo.ProviderKey, ProviderDisplayName = loginInfo.ProviderDisplayName, UserId = userId.Value };
                }
            }
            return null;
        }
    }
}
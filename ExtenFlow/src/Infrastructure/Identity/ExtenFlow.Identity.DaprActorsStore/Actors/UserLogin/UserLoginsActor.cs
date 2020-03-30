using System;
using System.Collections.Generic;
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
    public class UserLoginsActor : Actor, IUserLoginsActor
    {
        private const string _stateName = "UserLogins";
        private HashSet<UserLoginInfo>? _state;
        private IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        private HashSet<UserLoginInfo> State => _state ?? (_state = new HashSet<UserLoginInfo>());

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginsActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserLoginsActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<HashSet<UserLoginInfo>?>(_stateName);
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Gets the user login.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="providerKey">The provider key.</param>
        /// <returns>The user login info object.</returns>
        public Task<UserLoginInfo> GetUserLogin(string loginProvider, string providerKey)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Task.FromException<UserLoginInfo>(new ArgumentNullException(nameof(loginProvider)));
            }
            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return Task.FromException<UserLoginInfo>(new ArgumentNullException(nameof(providerKey)));
            }
            UserLoginInfo? login = State.Where(p => p.LoginProvider.Equals(loginProvider) && p.ProviderKey.Equals(providerKey)).FirstOrDefault();
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
        public Task<UserLoginInfo?> FindUserLogin(string loginProvider, string providerKey)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Task.FromException<UserLoginInfo?>(new ArgumentNullException(nameof(loginProvider)));
            }
            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return Task.FromException<UserLoginInfo?>(new ArgumentNullException(nameof(providerKey)));
            }
            return Task.FromResult<UserLoginInfo?>(State.Where(p => p.LoginProvider.Equals(loginProvider) && p.ProviderKey.Equals(providerKey)).FirstOrDefault());
        }
    }
}
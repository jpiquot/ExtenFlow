using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;
using ExtenFlow.Identity.Properties;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The user tokens class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserTokensActor"/>
    public class UserTokensActor : BaseActor<Dictionary<string, Dictionary<string, string>>>, IUserTokensActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserTokensActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserTokensActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Adds the specified token.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">loginProvider is null</exception>
        /// <exception cref="ArgumentNullException">name is null</exception>
        /// <exception cref="ArgumentNullException">value is null</exception>
        /// <exception cref="InvalidOperationException">Duplicate token</exception>
        public Task Add(string loginProvider, string name, string value)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Task.FromException(new ArgumentNullException(nameof(loginProvider)));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                return Task.FromException(new ArgumentNullException(nameof(name)));
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                return Task.FromException(new ArgumentNullException(nameof(value)));
            }
            var tokens = GetTokens(loginProvider);
            if (tokens.ContainsKey(name))
            {
                return Task.FromException(new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateToken, Id.GetId(), loginProvider, name)));
            }
            tokens.Add(name, value);
            SetState();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Finds the token value.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">loginProvider is null</exception>
        /// <exception cref="ArgumentNullException">name is null</exception>
        public Task<string?> FindValue(string loginProvider, string name)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Task.FromException<string?>(new ArgumentNullException(nameof(loginProvider)));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                return Task.FromException<string?>(new ArgumentNullException(nameof(name)));
            }
            if (GetTokens(loginProvider).TryGetValue(name, out string? value))
            {
                return Task.FromResult<string?>(value);
            }
            return Task.FromResult<string?>(null);
        }

        /// <summary>
        /// Removes the specified token.
        /// </summary>
        /// <param name="loginProvider">The login provider.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">loginProvider is null</exception>
        /// <exception cref="ArgumentNullException">name is null</exception>
        /// <exception cref="KeyNotFoundException">Token not found</exception>
        public Task Remove(string loginProvider, string name)
        {
            if (string.IsNullOrWhiteSpace(loginProvider))
            {
                return Task.FromException(new ArgumentNullException(nameof(loginProvider)));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                return Task.FromException(new ArgumentNullException(nameof(name)));
            }
            var tokens = GetTokens(loginProvider);
            if (!tokens.ContainsKey(name))
            {
                return Task.FromException(new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.TokenNotFound, Id.GetId(), loginProvider, name)));
            }
            tokens.Remove(name);
            SetState();
            return Task.CompletedTask;
        }

        private Dictionary<string, string> GetTokens(string loginProvider)
        {
            if (!State.TryGetValue(loginProvider, out Dictionary<string, string>? tokens))
            {
                tokens = new Dictionary<string, string>();
            }
            return tokens;
        }
    }
}
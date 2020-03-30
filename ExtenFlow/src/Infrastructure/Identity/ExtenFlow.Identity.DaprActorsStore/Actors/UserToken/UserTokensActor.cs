using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user tokens class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserTokensActor"/>
    public class UserTokensActor : Actor, IUserTokensActor
    {
        private const string _stateName = "UserTokens";
        private HashSet<string>? _state;

        private HashSet<string> State => _state ?? (_state = new HashSet<string>());

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

        private ITokenUsersActor GetTokenUsersActor(string tokenType) => ActorProxy.Create<ITokenUsersActor>(new ActorId(tokenType), nameof(TokenUsersActor));

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<HashSet<string>?>(_stateName);
            await base.OnActivateAsync();
        }

        /// <summary>
        /// Determines whether the user has the specified Token.
        /// </summary>
        /// <param name="tokenType">Type of the token</param>
        /// <param name="tokenValue">Value of the token</param>
        /// <returns>True if the user has the Token</returns>
        /// <exception cref="ArgumentNullException">tokenType</exception>
        public Task<bool> HasToken(string tokenType, string tokenValue)
        {
            if (string.IsNullOrWhiteSpace(tokenType))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(tokenType)));
            }
            if (!State.Any(p => p.Equals(tokenType)))
            {
                return Task.FromResult(false);
            }
            return GetTokenUsersActor(tokenType).HasUser(Id.GetId(), tokenValue);
        }

        /// <summary>
        /// Adds the user's Token.
        /// </summary>
        /// <param name="tokenType">Type of the token</param>
        /// <param name="tokenValue">Value of the token</param>
        /// <exception cref="ArgumentNullException">tokenType</exception>
        public async Task AddToken(string tokenType, string tokenValue)
        {
            if (State.Where(p => p.Equals(tokenType)).Any())
            {
                throw new InvalidOperationException($"The user is already in Token '{tokenType}'.");
            }
            State.Add(tokenType);
            await StateManager.SetStateAsync(_stateName, _state);
            await GetTokenUsersActor(tokenType).AddUser(Id.GetId(), tokenValue);
        }

        /// <summary>
        /// Removes the Token.
        /// </summary>
        /// <param name="tokenType">Type of the token</param>
        /// <param name="tokenValue">Value of the token</param>
        /// <exception cref="ArgumentNullException">tokenType</exception>
        public async Task RemoveToken(string tokenType, string tokenValue)
        {
            if (!State.Where(p => p.Equals(tokenType)).Any())
            {
                throw new InvalidOperationException($"The user does not have the Token '{tokenType}'.");
            }
            await GetTokenUsersActor(tokenType).RemoveUser(Id.GetId(), tokenValue);
            State.Remove(tokenType);
            await StateManager.SetStateAsync(_stateName, _state);
        }

        /// <summary>
        /// Gets the all the user's Tokens.
        /// </summary>
        /// <returns>A list of all Tokens</returns>
        public async Task<IList<Tuple<string, string>>> GetTokens()
        {
            var tokens = new List<Tuple<string, string>>();
            foreach (string tokenType in State)
            {
                foreach (string tokenValue in await GetTokenUsersActor(tokenType).GetTokenValues(Id.GetId()))
                {
                    tokens.Add(new Tuple<string, string>(tokenType, tokenValue));
                }
            }
            return tokens;
        }
    }
}
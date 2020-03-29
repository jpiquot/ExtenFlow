using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The User Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserActor"/>
    public class UserActor : Actor, IUserActor
    {
        private const string StateName = "User";
        private User? _state;
        private IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        private IUserNormalizedNameIndexActor GetUserNormilizedNameIndexActor(string userId) => ActorProxy.Create<IUserNormalizedNameIndexActor>(new ActorId(userId), nameof(UserNormalizedNameIndexActor));

        private User State => _state ?? throw new NullReferenceException(nameof(_state));

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Get the user name
        /// </summary>
        /// <returns>The user name</returns>
        public Task<string> GetUserName()
            => Task.FromResult(State?.UserName);

        /// <summary>
        /// Find user id
        /// </summary>
        /// <returns>Returns the user id or null if it does not exist</returns>
        public Task<string> FindUserId()
             => Task.FromResult(_state?.Id);

        /// <summary>
        /// Get the user normalized name
        /// </summary>
        /// <returns>The user normalized name</returns>
        public Task<string> GetNormalizedUserName()
          => Task.FromResult(State?.NormalizedUserName);

        /// <summary>
        /// Set the user name
        /// </summary>
        /// <param name="userName">The new user name</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">userName</exception>
        public async Task SetUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            var user = new User(State)
            {
                UserName = userName
            };
            IdentityResult result = await Update(user);
            if (!result.Succeeded)
            {
                throw new Exception($"Error while changing user ({Id.GetId()}) name to '{userName}' :" + string.Join("; ", result.Errors.Select(p => p.Description)));
            }
        }

        /// <summary>
        /// Set the user normalized name
        /// </summary>
        /// <param name="normalizedName">The new normalized name</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">normalizedName</exception>
        public async Task SetNormalizedUserName(string normalizedName)
        {
            if (string.IsNullOrWhiteSpace(normalizedName))
            {
                throw new ArgumentNullException(nameof(normalizedName));
            }
            var user = new User(State)
            {
                NormalizedUserName = normalizedName
            };
            IdentityResult result = await Update(user);
            if (!result.Succeeded)
            {
                throw new Exception($"Error while changing user ({Id.GetId()}) name to '{normalizedName}' :" + string.Join("; ", result.Errors.Select(p => p.Description)));
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Create(User user)
        {
            if (_state != null)
            {
                return IdentityResult.Failed(new[] { _errorDescriber.DuplicateUserName(user.Id) });
            }
            await GetUserNormilizedNameIndexActor(user.NormalizedUserName).Index(user.Id);
            try
            {
                user.ConcurrencyStamp = Guid.NewGuid().ToString();
                await StateManager.SetStateAsync(StateName, user);
                _state = user;
            }
            catch
            {
                await GetUserNormilizedNameIndexActor(user.NormalizedUserName).Remove(State.Id);
                throw;
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Update user properties
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        /// <exception cref="KeyNotFoundException">
        /// The user ({Id.GetId()}, does not exist or has been deleted.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The user Id ({user.Id}) is not the same as the actor Id({Id.GetId()})
        /// </exception>
        public async Task<IdentityResult> Update(User user)
        {
            if (_state == null)
            {
                throw new KeyNotFoundException($"The user ({Id.GetId()}, does not exist or has been deleted.");
            }
            if (user?.Id != Id.GetId())
            {
                throw new InvalidOperationException($"The user Id ({user?.Id}) is not the same as the actor Id({Id.GetId()})");
            }
            if (user.ConcurrencyStamp != State.ConcurrencyStamp)
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            if (!user.NormalizedUserName.Equals(State.NormalizedUserName))
            {
                string oldName = State.NormalizedUserName;
                await GetUserNormilizedNameIndexActor(user.NormalizedUserName).Index(State.Id);
                try
                {
                    await StateManager.SetStateAsync(StateName, user);
                    _state = user;
                }
                catch
                {
                    await GetUserNormilizedNameIndexActor(user.NormalizedUserName).Remove(State.Id);
                    throw;
                }
                await GetUserNormilizedNameIndexActor(oldName).Remove(State.Id);
            }
            else
            {
                await StateManager.SetStateAsync(StateName, user);
                _state = user;
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <returns>The operation result</returns>
        /// <exception cref="KeyNotFoundException">The user Id ({Id.GetId()}) already exist.</exception>
        public async Task<IdentityResult> Delete(string concurrencyStamp)
        {
            if (_state == null)
            {
                throw new KeyNotFoundException($"The user Id ({Id.GetId()}) already exist.");
            }
            if (concurrencyStamp != State.ConcurrencyStamp)
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            await StateManager.SetStateAsync(StateName, (User?)null);
            string normalizedName = _state.NormalizedUserName;
            _state = null;
            await GetUserNormilizedNameIndexActor(normalizedName).Remove(State.Id);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Getthe user
        /// </summary>
        /// <returns>The user object</returns>
        public Task<User> GetUser() => _state == null ? Task.FromException<User>(new KeyNotFoundException($"The user with Id='{Id.GetId()}' has not been created or has been deleted.")) : Task.FromResult(State);

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers.
        /// This method is called right after the actor is activated and before any method call or
        /// reminders are dispatched on it.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            _state = await StateManager.GetStateAsync<User?>(StateName);
            await base.OnActivateAsync();
        }
    }
}
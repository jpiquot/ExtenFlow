using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Security.Users;

using Microsoft.AspNetCore.Identity;

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.

namespace ExtenFlow.Security.DaprStore.Actors
{
    public class UserActor : Actor, IUserActor
    {
        private const string StateName = "User";
        private User? _state;
        private IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        public User State => _state ?? throw new NullReferenceException(nameof(_state));

        public UserActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        public Task<string> GetUserName()
            => Task.FromResult(State?.UserName);

        public Task<string> FindUserId()
             => Task.FromResult(_state?.Id);

        public Task<string> GetNormalizedUserName()
          => Task.FromResult(State?.NormalizedUserName);

        public Task SetUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            State.UserName = userName;

            return Save();
        }

        public Task SetNormalizedUserName(string normalizedName)
        {
            if (string.IsNullOrWhiteSpace(normalizedName))
            {
                throw new ArgumentNullException(nameof(normalizedName));
            }
            State.NormalizedUserName = normalizedName;

            return Save();
        }

        public async Task<IdentityResult> Create(User user)
        {
            if (_state != null)
            {
                return IdentityResult.Failed(new[] { _errorDescriber.DuplicateUserName(user.Id) });
            }
            _state = user;
            await Save();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> Update(User user)
        {
            if (_state == null)
            {
                throw new KeyNotFoundException($"The user ({Id.GetId()}, does not exist or has been deleted.");
            }
            if (user.Id != Id.GetId())
            {
                throw new InvalidOperationException($"The user Id ({user.Id}) is not the same as the actor Id({Id.GetId()})");
            }
            _state = user;
            await Save();
            return IdentityResult.Success;
        }

        private Task Save() => StateManager.SetStateAsync(StateName, _state);

        protected async Task Read() => _state = await StateManager.GetStateAsync<User>(StateName);

        public async Task<IdentityResult> Delete()
        {
            if (_state == null)
            {
                throw new KeyNotFoundException($"The user Id ({Id.GetId()}) already exist.");
            }
            _state = null;
            await Save();
            return IdentityResult.Success;
        }

        public Task<User> GetUser() => _state == null ? Task.FromException<User>(new KeyNotFoundException($"The user with Id='{Id.GetId()}' has not been created or has been deleted.")) : Task.FromResult(State);

        protected override async Task OnActivateAsync()
        {
            await Read();
            await base.OnActivateAsync();
        }
    }
}
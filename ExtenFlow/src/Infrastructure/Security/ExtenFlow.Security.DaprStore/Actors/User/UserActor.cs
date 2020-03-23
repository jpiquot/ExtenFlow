using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;
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
        private IdentityErrorDescriber _errorDescriber;

        public User State => _state ?? throw new NullReferenceException(nameof(_state));

        public IdentityErrorDescriber ErrorDescriber { get => _errorDescriber ?? (_errorDescriber = new IdentityErrorDescriber()); }

        public UserActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period) => throw new NotImplementedException();

        public Task<string?> FindUserId()
            => Task.FromResult(State?.Id);

        public Task<string> GetUserName()
            => Task.FromResult(State?.UserName);

        public Task<string> GetNormalizedUserName()
          => Task.FromResult(State?.NormalizedUserName);

        public async Task SetNormalizedUserName(string normalizedName)
        {
            IUserNormalizedNameIndexActor actor = ActorProxy.Create<IUserNormalizedNameIndexActor>(new ActorId(normalizedName), nameof(UserNormalizedNameIndexActor));
            await actor.Index(State.Id);
            State.NormalizedUserName = normalizedName;
        }

        public Task SetUserName(string userName)
        {
            State.UserName = (userName ?? throw new NullReferenceException(nameof(_state)));
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> Create(User user)
        {
            if (_state != null)
            {
                return IdentityResult.Failed(new IdentityError[] { ErrorDescriber.DuplicateUserName(user.Id) });
            }
            if (user.Id != Id.GetId())
            {
                return IdentityResult.Failed(new IdentityError[] { ErrorDescriber.(user.Id) });
                throw new InvalidOperationException($"The user Id ({user.Id}) is not the same as the actor Id({Id.GetId()})");
            }
            await IndexNormalizedName(user.Id);
            _state = user;
            await Save();
        }

        public async Task<IdentityResult> Update(User user)
        {
            if (_state != null)
            {
                throw new InvalidOperationException($"The user ({Id.GetId()}, does not exist or has been deleted.");
            }
            if (user.Id != Id.GetId())
            {
                throw new InvalidOperationException($"The user Id ({user.Id}) is not the same as the actor Id({Id.GetId()})");
            }
            await IndexNormalizedName(user.Id);
            _state = user;
            await Save();
        }

        private Task Save() => StateManager.SetStateAsync(StateName, _state);

        private async Task Read() => _state = await StateManager.GetStateAsync<User?>(StateName);

        private async Task IndexNormalizedName(string newNormalizedName)
        {
            if (string.IsNullOrWhiteSpace(newNormalizedName))
            {
                throw new ArgumentNullException(newNormalizedName);
            }
            if (_state?.NormalizedUserName != newNormalizedName)
            {
                if (_state != null)
                {
                    IUserNormalizedNameIndexActor oldIndex = ActorProxy.Create<IUserNormalizedNameIndexActor>(new ActorId(State.NormalizedUserName), nameof(UserNormalizedNameIndexActor));
                    await oldIndex.Remove(State.Id);
                }
                IUserNormalizedNameIndexActor index = ActorProxy.Create<IUserNormalizedNameIndexActor>(new ActorId(newNormalizedName), nameof(UserNormalizedNameIndexActor));
                await index.Index(State.Id);
            }
        }

        public async Task<IdentityResult> Delete()
        {
            IUserNormalizedNameIndexActor oldIndex = ActorProxy.Create<IUserNormalizedNameIndexActor>(new ActorId(State.NormalizedUserName), nameof(UserNormalizedNameIndexActor));
            await oldIndex.Remove(State.Id);
            _state = null;
            await Save();
        }

        public Task<User> GetUser() => Task.FromResult(State);
    }
}
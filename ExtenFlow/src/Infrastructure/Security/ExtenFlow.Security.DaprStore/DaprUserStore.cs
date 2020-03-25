using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using ExtenFlow.Security.DaprStore.Actors;
using ExtenFlow.Security.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

#pragma warning disable IDE1006 // Naming Styles

namespace ExtenFlow.Security.DaprStore
{
    public sealed class DaprUserStore :
        IUserStore<User>
    //IUserClaimStore<User>,
    //IUserLoginStore<User>,
    //IUserRoleStore<User>,
    //IUserPasswordStore<User>,
    //IUserSecurityStampStore<User>
    {
        private readonly IStringLocalizer<DaprUserStore> T;

        public DaprUserStore(IStringLocalizer<DaprUserStore> stringLocalizer)
        {
            T = stringLocalizer;
        }

        private IUserActor GetUserActor(string userId) => ActorProxy.Create<IUserActor>(new ActorId(userId), nameof(UserActor));

        private IUserNormalizedNameIndexActor GetUserNameIndexActor(string userId) => ActorProxy.Create<IUserNormalizedNameIndexActor>(new ActorId(userId), nameof(UserNormalizedNameIndexActor));

        public void Dispose()
        {
        }

        #region User Store

        async Task<string> IUserStore<User>.GetUserIdAsync(User user, CancellationToken cancellationToken)
            => (await GetUserActor(user.Id).FindUserId()) ?? throw new KeyNotFoundException(T["User with Id=%0", user.Id]);

        Task<string> IUserStore<User>.GetUserNameAsync(User user, CancellationToken cancellationToken) => GetUserActor(user.Id).GetUserName();

        Task IUserStore<User>.SetUserNameAsync(User user, string userName, CancellationToken cancellationToken) => GetUserActor(user.Id).SetUserName(userName);

        Task<string> IUserStore<User>.GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken) => GetUserActor(user.Id).GetNormalizedUserName();

        Task IUserStore<User>.SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken) => GetUserActor(user.Id).SetNormalizedUserName(normalizedName);

        Task<IdentityResult> IUserStore<User>.CreateAsync(User user, CancellationToken cancellationToken) => GetUserActor(user.Id).Create(user);

        Task<IdentityResult> IUserStore<User>.UpdateAsync(User user, CancellationToken cancellationToken) => GetUserActor(user.Id).Update(user);

        Task<IdentityResult> IUserStore<User>.DeleteAsync(User user, CancellationToken cancellationToken) => GetUserActor(user.Id).Delete();

        Task<User> IUserStore<User>.FindByIdAsync(string userId, CancellationToken cancellationToken)
            => GetUserActor(userId).GetUser();

        async Task<User> IUserStore<User>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            => await GetUserActor(await GetUserNameIndexActor(normalizedUserName).GetId() ?? throw new KeyNotFoundException(nameof(User.NormalizedUserName) + "=" + normalizedUserName)).GetUser();

        #endregion User Store
    }
}
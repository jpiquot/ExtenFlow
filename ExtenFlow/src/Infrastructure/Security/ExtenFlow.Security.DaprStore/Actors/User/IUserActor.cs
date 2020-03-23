using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Security.Users;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Security.DaprStore.Actors
{
    public interface IUserActor : IActor, IRemindable
    {
        Task<string?> FindUserId();

        Task<string> GetUserName();

        Task SetUserName(string userName);

        Task<string> GetNormalizedUserName();

        Task SetNormalizedUserName(string normalizedName);

        Task<IdentityResult> Create(User user);

        Task<IdentityResult> Update(User user);

        Task<IdentityResult> Delete();

        Task<User> GetUser();
    }
}
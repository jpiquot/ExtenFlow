using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Security.DaprStore.Actors
{
    public interface IUserClaimActor : IActor, IRemindable
    {
        Task SetPasswordHash(string passwordHash, CancellationToken cancellationToken);

        Task<string> GetPasswordHash(string id, CancellationToken cancellationToken);
    }
}
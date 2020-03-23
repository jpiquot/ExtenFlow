using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

namespace ExtenFlow.Security.DaprStore.Actors
{
    public interface IUserNormalizedNameIndexActor : IActor, IRemindable
    {
        Task<string?> GetId();

        Task Index(string id);

        Task Remove(string id);
    }
}
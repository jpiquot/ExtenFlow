using System.Threading.Tasks;

using Dapr.Actors;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user tokens actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IUserTokensActor : IActor
    {
        Task Add(string loginProvider, string name, string value);

        Task Remove(string loginProvider, string name);

        Task<string?> FindValue(string loginProvider, string name);
    }
}
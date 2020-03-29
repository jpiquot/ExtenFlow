using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The role actor interface
    /// </summary>
    /// <seealso cref="Dapr.Actors.IActor"/>
    public interface IRoleActor : IActor
    {
        Task<Role> GetRole();
    }
}
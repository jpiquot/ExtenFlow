using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Actors;
using ExtenFlow.IdentityServer.Domain;

namespace ExtenFlow.IdentityServer.Application.Actors
{
    /// <summary>
    /// The client actor interface
    /// </summary>
    /// <seealso cref="IActor"/>
    public interface IClientActor : IAggregateRootActor
    {
    }
}
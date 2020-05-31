using System.Threading.Tasks;

using ExtenFlow.Messages;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Interface IAggregateRootActor Implements the <see cref="Dapr.Actors.IActor"/>
    /// </summary>
    /// <seealso cref="Dapr.Actors.IActor"/>
    public interface IAggregateRootActor : IDispatchActor
    {
    }
}
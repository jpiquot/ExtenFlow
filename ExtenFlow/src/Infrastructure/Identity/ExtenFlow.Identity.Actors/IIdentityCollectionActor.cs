using Dapr.Actors;

using ExtenFlow.Services;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// Collection actor for identity actors
    /// </summary>
    /// <seealso cref="IActor"/>
    /// <seealso cref="ICollectionService"/>
    public interface IIdentityCollectionActor : IActor, ICollectionService
    {
    }
}
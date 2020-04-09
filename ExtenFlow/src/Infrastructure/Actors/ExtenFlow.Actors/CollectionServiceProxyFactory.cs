using Dapr.Actors;
using Dapr.Actors.Runtime;
using ExtenFlow.Services;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Actor collection proxy factory
    /// </summary>
    public static class CollectionServiceProxyFactory
    {
        /// <summary>
        /// Creates a collection service proxy.
        /// </summary>
        /// <typeparam name="TCollection">The collection actor</typeparam>
        /// <typeparam name="TItem">The collection item actor</typeparam>
        /// <returns>A new collection service.</returns>
        public static ICollectionService Create<TCollection, TItem>()
            where TItem : Actor
            where TCollection : ICollectionService, IActor
            => new CollectionServiceProxy<TCollection>(ActorHelper.ActorName(typeof(TItem)));
    }
}
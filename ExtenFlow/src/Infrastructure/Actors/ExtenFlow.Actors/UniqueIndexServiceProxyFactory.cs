using Dapr.Actors;
using Dapr.Actors.Runtime;
using ExtenFlow.Services;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Actor collection proxy factory
    /// </summary>
    public static class UniqueIndexServiceProxyFactory
    {
        /// <summary>
        /// Creates a collection service proxy.
        /// </summary>
        /// <typeparam name="TUniqueIndex">The collection actor</typeparam>
        /// <typeparam name="TItem">The collection item actor</typeparam>
        /// <returns>A new collection service.</returns>
        public static IUniqueIndexService Create<TUniqueIndex, TItem>()
            where TItem : Actor
            where TUniqueIndex : IUniqueIndexService, IActor
            => new UniqueIndexServiceProxy<TUniqueIndex>(ActorHelper.ActorName(typeof(TItem)));
    }
}
using Dapr.Actors.Runtime;

namespace ExtenFlow.Actors
{
    public static class CollectionServiceFactory
    {
        private ICollectionService Create<T>() where T : Actor => new CollectionServiceProxy(typeof(T));
    }
}
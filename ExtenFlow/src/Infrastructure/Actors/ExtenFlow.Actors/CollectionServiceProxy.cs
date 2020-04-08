using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

namespace ExtenFlow.Actors
{
    internal class CollectionServiceProxy<T> : ICollectionService where T : IActor, ICollectionService
    {
        private readonly string _collectionName;
        private ICollectionService? _actor;

        public CollectionServiceProxy(string collectionName) => _collectionName = collectionName;

        protected ICollectionService ProxyService => _actor ?? (_actor = ActorProxy.Create<T>(new ActorId(_collectionName), GetActorName()));

        public Task Add(string id) => ProxyService.Add(id);

        public Task<bool> Exist(string id) => ProxyService.Exist(id);

        public Task Remove(string id) => ProxyService.Remove(id);

        private string GetActorName()
        {
            string name = typeof(T).Name;
            if (name?[0] != 'I')
            {
                throw new InvalidOperationException(Properties.Resources.ActorTypeNotInterface);
            }
            return name.Substring(1);
        }
    }
}
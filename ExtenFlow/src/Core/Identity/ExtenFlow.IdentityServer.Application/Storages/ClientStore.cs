using System;
using System.Threading.Tasks;

using ExtenFlow.Actors;
using ExtenFlow.IdentityServer.Application.Actors;

using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace ExtenFlow.IdentityServer.Application.Storages
{
    public class ClientStore : IClientStore
    {
        private readonly IActorSystem _actorSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientStore"/> class.
        /// </summary>
        /// <param name="actorSystem">The actor system.</param>
        /// <exception cref="System.ArgumentNullException">actorSystem</exception>
        public ClientStore(IActorSystem actorSystem)
        {
            _actorSystem = actorSystem ?? throw new ArgumentNullException(nameof(actorSystem));
        }

        /// <summary>
        /// Finds a client by id
        /// </summary>
        /// <param name="clientId">The client id</param>
        /// <returns>The client</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var clientActor = _actorSystem.Create<IClientActor>(clientId);
            clientActor.Ask<ClientDe>
        }
    }
}
using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// Collection service actor for the identity module
    /// </summary>
    /// <seealso cref="ExtenFlow.Actors.CollectionActor"/>
    /// <seealso cref="ExtenFlow.Identity.Actors.IIdentityCollectionActor"/>
    public class IdentityCollectionActor : CollectionActor, IIdentityCollectionActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityCollectionActor"/> class.
        /// </summary>
        /// <param name="actorService">The actor service.</param>
        /// <param name="actorId">The actor identifier.</param>
        /// <param name="stateManager">The state manager.</param>
        public IdentityCollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? stateManager) : base(actorService, actorId, stateManager)
        {
        }
    }
}
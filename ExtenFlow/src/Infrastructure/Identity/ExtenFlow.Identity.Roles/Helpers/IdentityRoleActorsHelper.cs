using System;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Roles.Actors;
using ExtenFlow.Identity.Roles.Models;
using ExtenFlow.Messages.Dispatcher;

namespace ExtenFlow.Identity.Roles.Helpers
{
    /// <summary>
    /// Helper methods for identity actors.
    /// </summary>
    public static class IdentityRoleActorsHelper
    {
        /// <summary>
        /// Gets the user normalized name index actor.
        /// </summary>
        /// <returns>IUniqueIndexActor.</returns>
        public static IUniqueIndexActor GetRoleNormalizedNameIndexActor() => GetRoleUniqueIndexActor(nameof(RoleState.NormalizedName));

        /// <summary>
        /// Gets the index of the unique.
        /// </summary>
        /// <param name="aggregateField">The aggregate field.</param>
        /// <returns>IUniqueIndexActor.</returns>
        public static IUniqueIndexActor GetRoleUniqueIndexActor(string aggregateField)
            => ActorProxy.Create<IUniqueIndexActor>(new ActorId(nameof(Role) + "." + aggregateField), nameof(UniqueIndexActor));

        /// <summary>
        /// Registers the identity actors.
        /// </summary>
        /// <param name="actorRuntime">The actor runtime.</param>
        /// <param name="eventBus"></param>
        /// <param name="eventStore"></param>
        public static void RegisterIdentityRoleActors(this ActorRuntime actorRuntime, IEventBus eventBus, IEventStore eventStore)
        {
            if (actorRuntime == null)
            {
                throw new ArgumentNullException(nameof(actorRuntime));
            }
            actorRuntime.RegisterActor<RoleActor>(information
                => new ActorService(information, (service, id)
                    => new RoleActor(
                        service,
                        id,
                        GetRoleNormalizedNameIndexActor(),
                        eventBus,
                        eventStore
                        )
                ));
            actorRuntime.RegisterActor<RoleClaimsActor>(information
                => new ActorService(information, (service, id)
                    => new RoleClaimsActor(
                        service,
                        id,
                        eventBus,
                        eventStore
                        )
                ));
        }
    }
}
using System;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Users.Actors;
using ExtenFlow.Messages.Dispatcher;

namespace ExtenFlow.Identity.Users.Helpers
{
    /// <summary>
    /// Helper methods for identity actors.
    /// </summary>
    public static class IdentityUserActorsHelper
    {
        /// <summary>
        /// Gets the unique index for an aggregate field.
        /// </summary>
        /// <param name="aggregateType">Type of the aggregate.</param>
        /// <param name="aggregateField">The aggregate identifier.</param>
        /// <returns>IUniqueIndexActor.</returns>
        public static IUniqueIndexActor GetUniqueIndexActor(string aggregateType, string aggregateField)
            => ActorProxy.Create<IUniqueIndexActor>(new ActorId(aggregateType + "." + aggregateField), nameof(UniqueIndexActor));

        /// <summary>
        /// Gets the index of the unique.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="aggregateField">The aggregate field.</param>
        /// <returns>IUniqueIndexActor.</returns>
        public static IUniqueIndexActor GetUniqueIndexActor<TAggregate>(string aggregateField) => GetUniqueIndexActor(typeof(TAggregate).Name, aggregateField);

        /// <summary>
        /// Registers the identity actors.
        /// </summary>
        /// <param name="actorRuntime">The actor runtime.</param>
        /// <param name="eventBus"></param>
        /// <param name="eventStore"></param>
        public static void RegisterIdentityUserActors(this ActorRuntime actorRuntime, IEventBus eventBus, IEventStore eventStore)
        {
            if (actorRuntime == null)
            {
                throw new ArgumentNullException(nameof(actorRuntime));
            }
            actorRuntime.RegisterActor<UserActor>(information
                => new ActorService(information, (service, id)
                    => new UserActor(
                        service,
                        id,
                        GetUniqueIndexActor<User>(nameof(User.NormalizedName)),
                        eventBus,
                        eventStore
                        )
                ));
            actorRuntime.RegisterActor<UserClaimsActor>(information
                => new ActorService(information, (service, id)
                    => new UserClaimsActor(
                        service,
                        id,
                        eventBus,
                        eventStore
                        )
                ));
        }
    }
}
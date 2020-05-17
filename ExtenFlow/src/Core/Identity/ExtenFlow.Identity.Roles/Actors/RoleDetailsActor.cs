using System;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Domain;
using ExtenFlow.Domain.Dispatcher;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Roles.Events;
using ExtenFlow.Identity.Roles.Exceptions;
using ExtenFlow.Identity.Roles.Queries;

namespace ExtenFlow.Identity.Roles.Actors
{
    /// <summary>
    /// The Role Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IRoleActor"/>
    public class RoleDetailsActor : EventSourcedActorBase<RoleDetailsModel>, IRoleActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="messageQueue">The message queue used to publish events.</param>
        /// <param name="eventStore">The event store used to persist events.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleDetailsActor(
            ActorService actorService,
            ActorId actorId,
            IEventBus messageQueue,
            IEventStore eventStore,
            IActorStateManager? actorStateManager = null) : base(actorService, actorId, messageQueue, eventStore, actorStateManager)
        {
        }

        /// <summary>
        /// Creates new state.
        /// </summary>
        /// <returns>TState.</returns>
        protected override RoleDetailsModel NewState()
#pragma warning disable CS0618 // Type or member is obsolete
            => new RoleDetailsModel();

#pragma warning restore CS0618 // Type or member is obsolete

        /// <summary>
        /// Receives the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="batcheSave">if set to <c>true</c> [batche save].</param>
        /// <returns></returns>
        protected override async Task ReceiveEvent(IEvent @event, bool batcheSave = false)
        {
            switch (@event)
            {
                case NewRoleAdded create:
                    Apply(create);
                    break;

                case RoleRemoved delete:
                    Apply(delete);
                    break;

                case RoleRenamed rename:
                    Apply(rename);
                    break;

                default:
                    await base.ReceiveEvent(@event);
                    break;
            }
        }

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        protected override async Task<object> ReceiveQuery(IQuery query)
                    => query switch
                    {
                        GetRoleDetails getDetails => await Handle(getDetails),
                        _ => Task.FromException<object?>(new ArgumentOutOfRangeException(nameof(query)))
                    };

        private void Apply(RoleRenamed rename)
            => State = new RoleDetailsModel(Id.GetId(), rename.Name, rename.NormalizedName, State.ConcurrencyStamp);

        private void Apply(RoleRemoved _)
            => ClearState();

        private void Apply(NewRoleAdded create)
            => State = new RoleDetailsModel(Id.GetId(), create.Name, create.NormalizedName, State.ConcurrencyStamp);

        private Task<RoleDetailsModel> Handle(GetRoleDetails _)
        {
            if (StateIsNull())
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            return Task.FromResult(State);
        }
    }
}
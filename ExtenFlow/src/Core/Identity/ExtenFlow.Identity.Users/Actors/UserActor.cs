using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.EventStorage;
using ExtenFlow.Identity.Users.Commands;
using ExtenFlow.Identity.Users.Events;
using ExtenFlow.Identity.Users.Exceptions;
using ExtenFlow.Identity.Users.Models;
using ExtenFlow.Identity.Users.Properties;
using ExtenFlow.Identity.Users.Queries;
using ExtenFlow.Domain;
using ExtenFlow.Domain.Dispatcher;

namespace ExtenFlow.Identity.Users.Actors
{
    /// <summary>
    /// The User Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserActor"/>
    public class UserActor : EventSourcedActorBase<UserState>, IUserActor
    {
        private readonly IUniqueIndexActor _normalizedNameIndexActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="normalizedNameIndexActor"></param>
        /// <param name="messageQueue">The message queue used to publish events.</param>
        /// <param name="eventStore">The event store used to persist events.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserActor(
            ActorService actorService,
            ActorId actorId,
            IUniqueIndexActor normalizedNameIndexActor,
            IEventBus messageQueue,
            IEventStore eventStore,
            IActorStateManager? actorStateManager = null) : base(actorService, actorId, messageQueue, eventStore, actorStateManager)
        {
            _normalizedNameIndexActor = normalizedNameIndexActor ?? throw new ArgumentNullException(nameof(normalizedNameIndexActor));
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns>The user object</returns>
        public Task<User> GetUser()
        {
            if (StateIsNull())
            {
                return Task.FromException<User>(new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.UserNotFound, Id.GetId())));
            }
            return Task.FromResult(
                new User()
                {
                    Id = Id.GetId(),
                    UserName = State.UserName,
                    NormalizedUserName = State.NormalizedName,
                    ConcurrencyStamp = State.ConcurrencyStamp
                });
        }

        /// <summary>
        /// Creates new state.
        /// </summary>
        /// <returns>TState.</returns>
        protected override UserState NewState() => new UserState();

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        protected override Task<IList<IEvent>> ReceiveCommand(ICommand command)
            => command switch
            {
                RegisterNewUser create => Handle(create),
                UnregisterUser delete => Handle(delete),
                RenameUser rename => Handle(rename),
                _ => base.ReceiveCommand(command)
            };

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
                case NewUserRegistred create:
                    Apply(create);
                    break;

                case UserUnregistred delete:
                    Apply(delete);
                    break;

                case UserRenamed rename:
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
                        GetUserDetails create => await Handle(create),
                        _ => Task.FromException<object>(new ArgumentOutOfRangeException(nameof(query)))
                    };

        private void Apply(UserRenamed rename)
            => State.UserName = rename.Name;

        private void Apply(UserUnregistred _)
            => ClearState();

        private void Apply(NewUserRegistred create)
        {
            State.UserName = create.Name;
            State.NormalizedName = create.NormalizedName;
        }

        private Task<UserDetailsModel> Handle(GetUserDetails _)
        {
            if (StateIsNull())
            {
                throw new UserNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            return Task.FromResult(new UserDetailsModel(Id.GetId(), State.UserName ?? string.Empty, State.NormalizedName ?? string.Empty, State.ConcurrencyStamp));
        }

        /// <summary>
        /// Delete the user.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="UserNotFoundException">Id</exception>
        /// <exception cref="UserConcurrencyFailureException"></exception>
        private Task<IList<IEvent>> Handle(UnregisterUser command)
        {
            if (StateIsNull())
            {
                throw new UserNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            if (State.ConcurrencyStamp != command.ConcurrencyCheckStamp)
            {
                throw new UserConcurrencyFailureException(CultureInfo.CurrentCulture, command.ConcurrencyCheckStamp, State.ConcurrencyStamp);
            }
            return Task.FromResult<IList<IEvent>>(new[] { new UserUnregistred(Id.GetId(), command.UserId, command.CorrelationId) });
        }

        private Task<IList<IEvent>> Handle(RenameUser command)
        {
            if (StateIsNull())
            {
                throw new UserNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            if (State.ConcurrencyStamp != command.ConcurrencyCheckStamp)
            {
                throw new UserConcurrencyFailureException(CultureInfo.CurrentCulture, command.ConcurrencyCheckStamp, State.ConcurrencyStamp);
            }
            return Task.FromResult<IList<IEvent>>(new[] { new UserRenamed(Id.GetId(), command.Name, command.NormalizedName, command.UserId, command.CorrelationId) });
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="DuplicateUserException">Id</exception>
        private async Task<IList<IEvent>> Handle(RegisterNewUser command)
        {
            if (!StateIsNull())
            {
                throw new DuplicateUserException(CultureInfo.CurrentCulture, nameof(Id), Id.GetId());
            }
            if (await _normalizedNameIndexActor.Exist(command.NormalizedName))
            {
                throw new DuplicateUserException(CultureInfo.CurrentCulture, nameof(UserState.NormalizedName), command.NormalizedName);
            }
            return new[] { new NewUserRegistred(Id.GetId(), command.UserName, command.NormalizedName, command.UserId, command.CorrelationId) };
        }
    }
}
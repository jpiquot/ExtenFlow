using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ExtenFlow.Domain;
using ExtenFlow.Domain.Aggregates;
using ExtenFlow.Domain.Exceptions;
using ExtenFlow.Identity.Roles;
using ExtenFlow.IdentityServer.Domain.Commands;
using ExtenFlow.IdentityServer.Domain.Events;
using ExtenFlow.IdentityServer.Domain.Exceptions;
using ExtenFlow.IdentityServer.Domain.ValueObjects;
using ExtenFlow.Infrastructure.ValueObjects;
using ExtenFlow.Messages;

namespace ExtenFlow.IdentityServer.Domain
{
    /// <summary>
    /// The client aggregate root class. This class cannot be inherited. Implements the <see cref="AggregateRoot{RoleState}"/>
    /// </summary>
    /// <seealso cref="AggregateRoot{RoleState}"/>
    public sealed class ClientAggregateRoot : AggregateRoot<ClientState>
    {
        private ClientState? _clientState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAggregateRoot"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="repository">The repository.</param>
        public ClientAggregateRoot(string id, IRepository repository)
            : base(AggregateName.Client, id, repository)
        {
        }

        #region State

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The client state.</value>
        public ClientState ClientState
            => _clientState ?? throw new EntityStateNotInitializedException(this, nameof(ClientState));

        #endregion State

        #region Commands

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async override Task<IList<IEvent>> HandleCommand(ICommand command)
        {
            await InitializeState();
            IList<IEvent> events;
            switch (command)
            {
                case AddNewClient create:
                    events = Handle(create);
                    break;

                case RemoveClient delete:
                    events = Handle(delete);
                    break;

                case RenameRole rename:
                    events = Handle(rename);
                    break;

                default:
                    return await base.HandleCommand(command);
            };
            return events;
        }

        /// <summary>
        /// Delete the role.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="ClientNotFoundException">Id</exception>
        /// <exception cref="ClientConcurrencyFailureException"></exception>
        private IList<IEvent> Handle(RemoveClient command)
        {
            CheckConcurrencyStamp(command.ConcurrencyCheckStamp);
            return new[] { new ClientRemoved(Id, command.UserId, command.CorrelationId) };
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        private IList<IEvent> Handle(RenameRole command)
        {
            CheckConcurrencyStamp(command.ConcurrencyCheckStamp);
            return new[] { new RoleRenamed(Id, command.Name, command.NormalizedName, command.UserId, command.CorrelationId) };
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="DuplicateClientException">Id</exception>
        private IList<IEvent> Handle(AddNewClient command)
        {
            CheckCanCreate();
            return
                new[] { new NewClientAdded(
                    Id,
                    command.Name,
                    command.Description,
                    command.UserId,
                    command.CorrelationId)
                };
        }

        #endregion Commands

        #region Events

        /// <summary>
        /// Receives the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public override async Task HandleEvent(IEvent @event)
        {
            await InitializeState();
            switch (@event)
            {
                case NewClientAdded create:
                    Apply(create);
                    break;

                case ClientRemoved delete:
                    Apply(delete);
                    break;

                case RoleRenamed rename:
                    Apply(rename);
                    break;

                default:
                    return;
            };
            await Save();
        }

        private void Apply(RoleRenamed rename)
        {
            _name = new RoleName(rename.Name);
            _normalizedName = new RoleNormalizedName(rename.NormalizedName);
        }

        private void Apply(ClientRemoved _)
        {
            _name = null;
            _normalizedName = null;
            ClearConcurrencyCheckStamp();
        }

        private void Apply(NewClientAdded create)
        {
            _name = new RoleName(create.Name);
            _normalizedName = new RoleNormalizedName(create.NormalizedName);
            SetConcurrencyCheckStamp(create.ConcurrencyCheckStamp);
        }

        #endregion Events

        #region Queries

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        public async override Task<object> HandleQuery(IQuery query)
        {
            await InitializeState();
            return query switch
            {
                GetRoleDetails getDetails => Handle(getDetails),
                _ => await base.HandleQuery(query),
            };
            ;
        }

        private ClientDetails Handle(GetRoleDetails _)
            => new ClientDetails(Id, Name.Value, NormalizedName.Value, ConcurrencyCheckStamp.Value);

        #endregion Queries

        #region Notifications

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Task HandleNotification(IMessage message) => Task.CompletedTask;

        #endregion Notifications

        /// <summary>
        /// Gets the state object.
        /// </summary>
        /// <returns>The state object initialized with the instance values.</returns>
        protected override ClientState GetState()
            => new ClientState(Name.Value, NormalizedName.Value, ConcurrencyCheckStamp.Value);

        /// <summary>
        /// Sets the data values from the persisted state object.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="ArgumentNullException">state</exception>
        protected override void SetValues(ClientState state)
        {
            _ = state ?? throw new ArgumentNullException(nameof(state));
            _name = new RoleName(state.Name);
            _normalizedName = new RoleNormalizedName(state.NormalizedName);
            SetConcurrencyCheckStamp(state.ConcurrencyCheckStamp);
        }
    }
}
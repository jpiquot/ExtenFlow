using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ExtenFlow.Domain;
using ExtenFlow.Domain.Aggregates;
using ExtenFlow.Domain.Exceptions;
using ExtenFlow.Identity.Roles.Application.Queries;
using ExtenFlow.Identity.Roles.Domain.Commands;
using ExtenFlow.Identity.Roles.Domain.Events;
using ExtenFlow.Identity.Roles.Domain.Exceptions;
using ExtenFlow.Identity.Roles.Domain.ValueObjects;
using ExtenFlow.Infrastructure.ValueObjects;
using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Roles.Domain
{
    /// <summary>
    /// Class RoleAggregateRoot. This class cannot be inherited. Implements the <see cref="AggregateRoot{RoleState}"/>
    /// </summary>
    /// <seealso cref="AggregateRoot{RoleState}"/>
    public sealed class RoleAggregateRoot : AggregateRoot<RoleState>
    {
        private RoleName? _name;
        private RoleNormalizedName? _normalizedName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAggregateRoot"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="repository">The repository.</param>
        public RoleAggregateRoot(string id, IRepository repository)
            : base("Role", id, repository)
        {
        }

        #region State

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public RoleName Name
            => _name ?? throw new EntityStateNotInitializedException(this, nameof(Name));

        /// <summary>
        /// Gets or sets the normalized name of the role.
        /// </summary>
        /// <value>The normalized name.</value>
        public RoleNormalizedName NormalizedName
            => _normalizedName ?? throw new EntityStateNotInitializedException(this, nameof(NormalizedName));

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
                case AddNewRole create:
                    events = Handle(create);
                    break;

                case RemoveRole delete:
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
        /// <exception cref="RoleNotFoundException">Id</exception>
        /// <exception cref="RoleConcurrencyFailureException"></exception>
        private IList<IEvent> Handle(RemoveRole command)
        {
            CheckConcurrencyStamp(command.ConcurrencyCheckStamp);
            return new[] { new RoleRemoved(Id, command.UserId, command.CorrelationId) };
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
        /// <exception cref="DuplicateRoleException">Id</exception>
        private IList<IEvent> Handle(AddNewRole command)
        {
            CheckCanCreate();
            return
                new[] { new NewRoleAdded(
                    Id,
                    command.Name,
                    command.NormalizedName,
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
                    return;
            };
            await Save();
        }

        private void Apply(RoleRenamed rename)
        {
            _name = new RoleName(rename.Name);
            _normalizedName = new RoleNormalizedName(rename.NormalizedName);
        }

        private void Apply(RoleRemoved _)
        {
            _name = null;
            _normalizedName = null;
            ClearConcurrencyCheckStamp();
        }

        private void Apply(NewRoleAdded create)
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

        private RoleDetails Handle(GetRoleDetails _)
            => new RoleDetails(Id, Name.Value, NormalizedName.Value, ConcurrencyCheckStamp.Value);

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
        protected override RoleState GetState()
            => new RoleState(Name.Value, NormalizedName.Value, ConcurrencyCheckStamp.Value);

        /// <summary>
        /// Sets the data values from the persisted state object.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="ArgumentNullException">state</exception>
        protected override void SetValues(RoleState state)
        {
            _ = state ?? throw new ArgumentNullException(nameof(state));
            _name = new RoleName(state.Name);
            _normalizedName = new RoleNormalizedName(state.NormalizedName);
            SetConcurrencyCheckStamp(state.ConcurrencyCheckStamp);
        }
    }
}
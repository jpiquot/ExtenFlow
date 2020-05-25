using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using ExtenFlow.Domain;
using ExtenFlow.Domain.Aggregates;
using ExtenFlow.Domain.Exceptions;
using ExtenFlow.Identity.Roles.Commands;
using ExtenFlow.Identity.Roles.Events;
using ExtenFlow.Identity.Roles.Exceptions;
using ExtenFlow.Identity.Roles.Queries;
using ExtenFlow.Identity.Roles.ValueObjects;
using ExtenFlow.Infrastructure.ValueObjects;

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
        public override Task<IList<IEvent>> HandleCommand(ICommand command)
            => command switch
            {
                AddNewRole create => Handle(create),
                RemoveRole delete => Handle(delete),
                RenameRole rename => Handle(rename),
                _ => base.HandleCommand(command)
            };

        /// <summary>
        /// Delete the role.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="RoleNotFoundException">Id</exception>
        /// <exception cref="RoleConcurrencyFailureException"></exception>
        private async Task<IList<IEvent>> Handle(RemoveRole command)
        {
            await CheckConcurrencyStamp(command.ConcurrencyStamp);
            return new[] { new RoleRemoved(Id, command.UserId, command.CorrelationId) };
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        private async Task<IList<IEvent>> Handle(RenameRole command)
        {
            await CheckConcurrencyStamp(command.ConcurrencyStamp);
            return new[] { new RoleRenamed(Id, command.Name, command.NormalizedName, command.UserId, command.CorrelationId) };
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="DuplicateRoleException">Id</exception>
        private async Task<IList<IEvent>> Handle(AddNewRole command)
        {
            if (!_stateInitialized)
            {
                await ReadState();
            }
            if (_concurrencyStamp != null || _name != null || _normalizedName != null)
            {
                throw new DuplicateRoleException(CultureInfo.CurrentCulture, nameof(Id), Id);
            }
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
        public override Task HandleEvent(IEvent @event)
        => @event switch
        {
            NewRoleAdded create => Apply(create),
            RoleRemoved delete => Apply(delete),
            RoleRenamed rename => Apply(rename),
            _ => Task.CompletedTask
        };

        private async Task Apply(RoleRenamed rename)
        {
            _name = new RoleName(rename.Name);
            _normalizedName = new RoleNormalizedName(rename.NormalizedName);
            await Save();
        }

        private Task Apply(RoleRemoved _)
        {
            _name = null;
            _normalizedName = null;
            ConcurrencyStamp = null;
            return Save();
        }

        private Task Apply(NewRoleAdded create)
        {
            _name = new RoleName(create.Name);
            _normalizedName = new RoleNormalizedName(create.NormalizedName);
            _concurrencyStamp = null;
            return Save();
        }

        #endregion Events

        #region Queries

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        public async override Task<object> HandleQuery(IQuery query)
                    => query switch
                    {
                        GetRoleDetails getDetails => await Handle(getDetails),
                        _ => await base.HandleQuery(query)
                    };

        private async Task<RoleDetailsModel> Handle(GetRoleDetails _)
        {
            await CheckState();
            return new RoleDetailsModel(Id, Name.Value, NormalizedName.Value, ConcurrencyCheckStamp.Value);
        }

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
            ConcurrencyStamp = new ConcurrencyCheckStamp(state.ConcurrencyCheckStamp);
        }
    }
}
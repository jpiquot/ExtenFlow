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
    /// Class RoleAggregateRoot. Implements the <see cref="ExtenFlow.Domain.Aggregates.AggregateRoot"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.Aggregates.AggregateRoot"/>
    public sealed class RoleAggregateRoot : AggregateRoot
    {
        private ConcurrencyStamp? _concurrencyStamp;
        private Name? _name;
        private NormalizedName? _normalizedName;
        private bool _stateInitialized;

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
        /// Gets the concurrency stamp. A random value that should change whenever a role is
        /// persisted to the store.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        public ConcurrencyStamp ConcurrencyStamp
            => _concurrencyStamp ?? throw new EntityStateNotInitializedException(this, nameof(ConcurrencyStamp));

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public Name Name
            => _name ?? throw new EntityStateNotInitializedException(this, nameof(Name));

        /// <summary>
        /// Gets or sets the normalized name of the role.
        /// </summary>
        /// <value>The normalized name.</value>
        public NormalizedName NormalizedName
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
            _name = new Name(rename.Name);
            _normalizedName = new NormalizedName(rename.NormalizedName);
            await SaveRole();
        }

        private Task Apply(RoleRemoved _)
        {
            _name = null;
            _normalizedName = null;
            _concurrencyStamp = null;
            return SaveRole();
        }

        private Task Apply(NewRoleAdded create)
        {
            _name = new Name(create.Name);
            _normalizedName = new NormalizedName(create.NormalizedName);
            _concurrencyStamp = null;
            return SaveRole();
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
            return new RoleDetailsModel(Id, Name.Value, NormalizedName.Value, ConcurrencyStamp.Value);
        }

        #endregion Queries

        #region Notifications

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Task HandleNotification(IMessage message) => Task.CompletedTask;

        #endregion Notifications

        private async Task CheckConcurrencyStamp(string? concurrencyStamp)
        {
            await CheckState();
            if (ConcurrencyStamp.Value != concurrencyStamp)
            {
                throw new RoleConcurrencyFailureException(CultureInfo.CurrentCulture, concurrencyStamp, ConcurrencyStamp.Value);
            }
        }

        private async Task CheckState()
        {
            if (!_stateInitialized)
            {
                await ReadState();
            }
            if (_concurrencyStamp == null)
            {
                throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id);
            }
        }

        private async Task ReadState()
        {
            (bool result, RoleState state) = await Repository.TryGetData<RoleState>(RoleEntity.EntityName);
            if (result)
            {
                _concurrencyStamp = new ConcurrencyStamp(state.ConcurrencyStamp);
                _name = new Name(state.Name);
                _normalizedName = new NormalizedName(state.NormalizedName);
            }
            else
            {
                _concurrencyStamp = null;
                _name = null;
                _normalizedName = null;
            }
            _stateInitialized = true;
        }

        private Task SaveRole()
        {
            if (_concurrencyStamp == null && _name == null && _normalizedName == null)
            {
                return Repository.RemoveData(RoleEntity.EntityName);
            }
            _concurrencyStamp = new ConcurrencyStamp();
            return Repository.SetData(RoleEntity.EntityName, new RoleState(Name.Value, NormalizedName.Value, ConcurrencyStamp.Value));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using ExtenFlow.Domain;
using ExtenFlow.Domain.Aggregates;
using ExtenFlow.Identity.Roles.Commands;
using ExtenFlow.Identity.Roles.Domain;
using ExtenFlow.Identity.Roles.Events;
using ExtenFlow.Identity.Roles.Exceptions;
using ExtenFlow.Identity.Roles.Queries;

namespace ExtenFlow.Identity.Roles.Actors
{
    /// <summary>
    /// Class RoleAggregateRoot. Implements the <see cref="ExtenFlow.Domain.Aggregates.AggregateRoot"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.Aggregates.AggregateRoot"/>
    public sealed class RoleAggregateRoot : AggregateRoot
    {
        private RoleEntity? _role;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAggregateRoot"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="repository">The repository.</param>
        public RoleAggregateRoot(string id, IRepository repository)
            : base(nameof(Role), id, repository)
        {
        }

        private RoleEntity Role => _role ?? throw new InvalidOperationException(Properties.Resources.RoleNotInitialized);

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

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Task HandleNotification(IMessage message) => Task.CompletedTask;

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

        private async Task Apply(RoleRenamed rename)
        {
            await CheckRoleExist();
            Role.Apply(rename);
            await SaveRole();
        }

        private Task Apply(RoleRemoved _)
        {
            _role = null;
            return SaveRole();
        }

        private Task Apply(NewRoleAdded create)
        {
            _role = new RoleEntity(create.Name, create.NormalizedName);
            return SaveRole();
        }

        private async Task CheckRoleConcurrencyStamp(string? concurrencyStamp)
        {
            await CheckRoleExist();
            if (Role.ConcurrencyStamp.Value != concurrencyStamp)
            {
                throw new RoleConcurrencyFailureException(CultureInfo.CurrentCulture, concurrencyStamp, Role.ConcurrencyStamp.Value);
            }
        }

        private async Task CheckRoleExist()
        {
            if (_role == null)
            {
                (bool result, RoleState state) = await Repository.TryGetData<RoleState>(RoleEntity.EntityName);
                if (result)
                {
                    _role = new RoleEntity(state);
                }
                else
                {
                    throw new RoleNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id);
                }
            }
        }

        private async Task<RoleDetailsModel> Handle(GetRoleDetails _)
        {
            await CheckRoleExist();
            return new RoleDetailsModel(Id, Role.Name.Value, Role.NormalizedName.Value, Role.ConcurrencyStamp.Value);
        }

        /// <summary>
        /// Delete the role.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IList&lt;IEvent&gt;.</returns>
        /// <exception cref="RoleNotFoundException">Id</exception>
        /// <exception cref="RoleConcurrencyFailureException"></exception>
        private async Task<IList<IEvent>> Handle(RemoveRole command)
        {
            await CheckRoleConcurrencyStamp(command.ConcurrencyStamp);
            return new[] { new RoleRemoved(Id, command.UserId, command.CorrelationId) };
        }

        private async Task<IList<IEvent>> Handle(RenameRole command)
        {
            await CheckRoleConcurrencyStamp(command.ConcurrencyStamp);
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
            if (_role != null || await Repository.HasData(RoleEntity.EntityName))
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

        private Task SaveRole()
            => (_role == null) ?
                Repository.RemoveData(RoleEntity.EntityName) :
                Repository.SetData(RoleEntity.EntityName, Role.GetState());
    }
}
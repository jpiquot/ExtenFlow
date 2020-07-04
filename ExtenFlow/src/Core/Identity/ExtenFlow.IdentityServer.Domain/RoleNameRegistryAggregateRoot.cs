using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using ExtenFlow.Domain;
using ExtenFlow.Domain.Aggregates;
using ExtenFlow.IdentityServer.Application.Queries;
using ExtenFlow.IdentityServer.Domain.Commands;
using ExtenFlow.IdentityServer.Domain.Events;
using ExtenFlow.IdentityServer.Domain.Exceptions;
using ExtenFlow.IdentityServer.Domain.Models;
using ExtenFlow.IdentityServer.Domain.ValueObjects;
using ExtenFlow.Messages;

namespace ExtenFlow.IdentityServer.Domain
{
    /// <summary>
    /// Class RoleNameRegistryAggregateRoot. Implements the <see cref="ExtenFlow.Domain.Aggregates.AggregateRoot{T}"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.Aggregates.AggregateRoot{T}"/>
    public sealed class RoleNameRegistryAggregateRoot : AggregateRoot<RoleNameRegistryState>
    {
        private ClientId? _roleId;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNameRegistryAggregateRoot"/> class.
        /// </summary>
        /// <param name="normalizedName">The identifier.</param>
        /// <param name="repository">The repository.</param>
        public RoleNameRegistryAggregateRoot(string normalizedName, IRepository repository)
           : base("RoleNameRegistry", normalizedName, repository)
        {
            // Validate the aggregate identifier (normalized role name).
            _ = new RoleNormalizedName(normalizedName);
        }

        private ClientId RoleId => _roleId ?? throw new InvalidOperationException(Properties.Resources.RoleNotInitialized);

        #region Commands

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public override Task<IList<IEvent>> HandleCommand(ICommand command)
            => command switch
            {
                RegisterNormalizedRoleName register => Handle(register),
                DeregisterNormalizedRoleName deregister => Handle(deregister),
                _ => base.HandleCommand(command)
            };

        #endregion Commands

        #region Events

        /// <summary>
        /// Receives the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns></returns>
        public override Task HandleEvent(IEvent @event)
        =>
            @event switch
            {
                NormalizedRoleNameRegistred registered => Apply(registered),
                NormalizedRoleNameDeregistred deregistered => Apply(deregistered),
                _ => Task.CompletedTask
            };

        #endregion Events

        #region Notifications

        /// <summary>
        /// Handles notifications.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task HandleNotification(IMessage message) => Task.CompletedTask;

        #endregion Notifications

        #region Queries

        /// <summary>
        /// Receive a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        public override async Task<object> HandleQuery(IQuery query)
                    => query switch
                    {
                        GetRoleIdByName getId => await Handle(getId),
                        IsRoleNameRegistered exist => await Handle(exist),
                        _ => Task.FromException<object>(new ArgumentOutOfRangeException(nameof(query)))
                    };

        #endregion Queries

        /// <summary>
        /// Gets the state object.
        /// </summary>
        /// <returns>The state object initialized with the instance values.</returns>
        protected override RoleNameRegistryState GetState()
            => new RoleNameRegistryState(RoleId.Value, ConcurrencyCheckStamp.Value);

        /// <summary>
        /// Sets the data values from the persisted state object.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="ArgumentNullException">state</exception>
        protected override void SetValues(RoleNameRegistryState state)
        {
            _ = state ?? throw new ArgumentNullException(nameof(state));
            _roleId = new ClientId(state.RoleId);
        }

        private Task Apply(NormalizedRoleNameRegistred registred)
        {
            _roleId = new ClientId(registred.RoleId);
            return Save();
        }

        private Task Apply(NormalizedRoleNameDeregistred _)
        {
            _roleId = null;
            return Save();
        }

        private async Task CheckRole()
        {
            if (_roleId == null)
            {
                (bool succeded, string roleId) = await Repository.TryGetData<string>(EntityName);
                if (succeded)
                {
                    _roleId = new ClientId(roleId);
                }
                else
                {
                    throw new ClientNotFoundException(CultureInfo.CurrentCulture, nameof(Id), Id);
                }
            }
        }

        private async Task<string> Handle(GetRoleIdByName _)
        {
            await CheckRole();
            return RoleId.Value;
        }

        private async Task<bool> Handle(IsRoleNameRegistered _)
        {
            if (_roleId == null)
            {
                (bool succeded, string roleId) = await Repository.TryGetData<string>(EntityName);
                if (succeded)
                {
                    _roleId = new ClientId(roleId);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private async Task<IList<IEvent>> Handle(RegisterNormalizedRoleName command)
        {
            if (_roleId == null)
            {
                (bool succeded, string roleId) = await Repository.TryGetData<string>(EntityName);
                if (succeded)
                {
                    _roleId = new ClientId(roleId);
                }
                else
                {
                    return new[] { new NormalizedRoleNameRegistred(Id, command.RoleId, command.UserId, command.CorrelationId) };
                }
            }
            throw new DuplicateClientException(CultureInfo.CurrentCulture, nameof(Role.NormalizedName), Id);
        }

        private async Task<IList<IEvent>> Handle(DeregisterNormalizedRoleName command)
        {
            await CheckRole();
            return new[] { new NormalizedRoleNameDeregistred(Id, command.RoleId, command.UserId, command.CorrelationId) };
        }
    }
}
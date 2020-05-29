using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using ExtenFlow.Domain;
using ExtenFlow.Domain.Aggregates;
using ExtenFlow.Domain.Exceptions;
using ExtenFlow.Identity.Roles.Events;
using ExtenFlow.Identity.Roles.ValueObjects;
using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Roles.Domain
{
    /// <summary>
    /// The role claims class
    /// </summary>
    /// <seealso cref="Actor"/>
    public sealed class RoleUsersEntity : Entity<HashSet<string>>
    {
        private HashSet<RoleUserId>? _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleUsersEntity"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="repository">The repository.</param>
        public RoleUsersEntity(string id, IRepository repository)
            : base("RoleClaims", id, repository)
        {
        }

        private HashSet<RoleUserId> Users
            => _users ?? throw new EntityStateNotInitializedException(this, nameof(Users));

        #region Events

        /// <summary>
        /// Handles events.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public override async Task HandleEvent(IEvent @event)
        {
            switch (@event)
            {
                case UserAddedToRole userAdded:
                    await InitializeState();
                    Apply(userAdded);
                    break;

                case UserRemovedFromRole userRemoved:
                    await InitializeState();
                    Apply(userRemoved);
                    break;

                default:
                    return;
            };
            await Save();
        }

        private void Apply(UserRemovedFromRole @event)
            => Users.Remove(RoleUserId(@event.RoleUserId));

        private void Apply(UserAddedToRole @event)
            => Users.Add(RoleUserId(@event.RoleUserId));

        #endregion Events

        /// <summary>
        /// Gets the state object.
        /// </summary>
        /// <returns>The state object initialized with the instance values.</returns>
        protected override HashSet<string> GetState()
            => Users.Select(p => p.Value).ToHashSet();

        /// <summary>
        /// Sets the data values from the persisted state object.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="ArgumentNullException">state</exception>
        protected override void SetValues(HashSet<string> state)
        {
            _ = state ?? throw new ArgumentNullException(nameof(state));
            _users = state.Select(p => RoleUserId(p)).ToHashSet();
        }

        private RoleUserId RoleUserId(string userId) => new RoleUserId(userId, EntityName, nameof(Users));
    }
}
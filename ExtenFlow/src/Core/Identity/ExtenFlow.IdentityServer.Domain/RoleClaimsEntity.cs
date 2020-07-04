using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using ExtenFlow.Domain;
using ExtenFlow.Domain.Aggregates;
using ExtenFlow.Domain.Exceptions;
using ExtenFlow.IdentityServer.Domain.Events;
using ExtenFlow.IdentityServer.Domain.ValueObjects;
using ExtenFlow.Messages;

namespace ExtenFlow.IdentityServer.Domain
{
    /// <summary>
    /// The role claims class
    /// </summary>
    /// <seealso cref="Actor"/>
    public sealed class RoleClaimsEntity : Entity<Dictionary<string, HashSet<string?>>>
    {
        private Dictionary<ClientClaimType, HashSet<ClientClaimValue?>>? _claims;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimsEntity"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="repository">The repository.</param>
        public RoleClaimsEntity(string id, IRepository repository)
            : base("RoleClaims", id, repository)
        {
        }

        private Dictionary<ClientClaimType, HashSet<ClientClaimValue?>> Claims
            => _claims ?? throw new EntityStateNotInitializedException(this, nameof(Claims));

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
                case ClientClaimAdded claimAdded:
                    await InitializeState();
                    Apply(claimAdded);
                    break;

                case ClientClaimRemoved claimRemoved:
                    await InitializeState();
                    Apply(claimRemoved);
                    break;

                default:
                    return;
            };
            await Save();
        }

        private void Apply(ClientClaimRemoved @event)
            => ClaimValues(RoleClaimType(@event.ClaimType))
                .Remove((@event.ClaimValue == null) ? null : RoleClaimValue(@event.ClaimValue));

        private void Apply(ClientClaimAdded @event)
            => ClaimValues(RoleClaimType(@event.ClaimType))
                .Add((@event.ClaimValue == null) ? null : RoleClaimValue(@event.ClaimValue));

        #endregion Events

        /// <summary>
        /// Gets the state object.
        /// </summary>
        /// <returns>The state object initialized with the instance values.</returns>
        protected override Dictionary<string, HashSet<string?>> GetState()
        {
            var dictionary = new Dictionary<string, HashSet<string?>>(Claims.Count);
            foreach (KeyValuePair<ClientClaimType, HashSet<ClientClaimValue?>> claimType in Claims)
            {
                if (claimType.Value != null)
                {
                    dictionary.Add(
                        claimType.Key.Value,
                        new HashSet<string?>(claimType.Value.Select(p => p?.Value).ToList()
                        ));
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Sets the data values from the persisted state object.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="ArgumentNullException">state</exception>
        protected override void SetValues(Dictionary<string, HashSet<string?>> state)
        {
            _ = state ?? throw new ArgumentNullException(nameof(state));
            _claims = new Dictionary<ClientClaimType, HashSet<ClientClaimValue?>>(state.Count);
            foreach (KeyValuePair<string, HashSet<string?>> claimType in state)
            {
                if (claimType.Value != null)
                {
                    _claims.Add(
                        RoleClaimType(claimType.Key),
                        claimType
                            .Value
                            .Select(p => (p == null) ? null : RoleClaimValue(p))
                            .ToHashSet()
                        );
                }
            }
        }

        private HashSet<ClientClaimValue?> ClaimValues(ClientClaimType claimType)
        {
            if (!Claims.TryGetValue(claimType, out HashSet<ClientClaimValue?>? values))
            {
                values = new HashSet<ClientClaimValue?>();
                Claims.Add(claimType, values);
            }
            return values;
        }

        private HashSet<ClientClaimValue?> ClaimValues(string claimType)
            => ClaimValues(RoleClaimType(claimType));

        private ClientClaimType RoleClaimType(string claimType)
            => new ClientClaimType(claimType, EntityName, nameof(Claims));

        private ClientClaimValue RoleClaimValue(string claimValue)
            => new ClientClaimValue(claimValue, EntityName, nameof(Claims));
    }
}
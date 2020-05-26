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

namespace ExtenFlow.Identity.Roles.Domain
{
    /// <summary>
    /// The role claims class
    /// </summary>
    /// <seealso cref="Actor"/>
    public sealed class RoleClaimsEntity : Entity<Dictionary<string, HashSet<string?>>>
    {
        private Dictionary<RoleClaimType, HashSet<RoleClaimValue?>>? _claims;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimsEntity"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="repository">The repository.</param>
        public RoleClaimsEntity(string id, IRepository repository)
            : base("RoleClaims", id, repository)
        {
        }

        private Dictionary<RoleClaimType, HashSet<RoleClaimValue?>> Claims
            => _claims ?? throw new EntityStateNotInitializedException(this, nameof(Claims));

        /// <summary>
        /// Determines whether the role has the specified Claim.
        /// </summary>
        /// <param name="claimType">Type of the claim</param>
        /// <param name="claimValue">Value of the claim</param>
        /// <returns>True if the role has the Claim</returns>
        /// <exception cref="ArgumentNullException">claimType</exception>
        public Task<bool> Exist(string claimType, string? claimValue)
        {
            if (string.IsNullOrWhiteSpace(claimType))
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(claimType)));
            }
            return Task.FromResult(ClaimValues(claimType)
                .Any(p => (p == null && claimValue == null) || p?.Value == claimValue));
        }

        /// <summary>
        /// Gets the all the role's Claims.
        /// </summary>
        /// <returns>A list of all Claims</returns>
        public Task<IList<Tuple<string, string?>>> GetAll()
        {
            var list = new List<Tuple<string, string?>>();
            foreach (KeyValuePair<RoleClaimType, HashSet<RoleClaimValue?>> entry in Claims)
            {
                foreach (RoleClaimValue? value in entry.Value)
                {
                    list.Add(new Tuple<string, string?>(entry.Key.Value, value?.Value));
                }
            }
            return Task.FromResult<IList<Tuple<string, string?>>>(list);
        }

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
                case RoleClaimAdded claimAdded:
                    await InitializeState();
                    Apply(claimAdded);
                    break;

                case RoleClaimRemoved claimRemoved:
                    await InitializeState();
                    Apply(claimRemoved);
                    break;

                default:
                    return;
            };
            await Save();
        }

        private void Apply(RoleClaimRemoved @event)
            => ClaimValues(new RoleClaimType(@event.ClaimType))
                .Remove((@event.ClaimValue == null) ? null : new RoleClaimValue(@event.ClaimValue));

        private void Apply(RoleClaimAdded @event)
            => ClaimValues(new RoleClaimType(@event.ClaimType))
                .Add((@event.ClaimValue == null) ? null : new RoleClaimValue(@event.ClaimValue));

        #endregion Events

        /// <summary>
        /// Gets the state object.
        /// </summary>
        /// <returns>The state object initialized with the instance values.</returns>
        protected override Dictionary<string, HashSet<string?>> GetState()
        {
            var dictionary = new Dictionary<string, HashSet<string?>>(Claims.Count);
            foreach (KeyValuePair<RoleClaimType, HashSet<RoleClaimValue?>> claimType in Claims)
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
            _claims = new Dictionary<RoleClaimType, HashSet<RoleClaimValue?>>(state.Count);
            foreach (KeyValuePair<string, HashSet<string?>> claimType in state)
            {
                if (claimType.Value != null)
                {
                    _claims.Add(
                        new RoleClaimType(claimType.Key),
                        claimType
                            .Value
                            .Select(p => (p == null) ? null : new RoleClaimValue(p))
                            .ToHashSet()
                        );
                }
            }
        }

        private HashSet<RoleClaimValue?> ClaimValues(RoleClaimType claimType)
        {
            if (!Claims.TryGetValue(claimType, out HashSet<RoleClaimValue?>? values))
            {
                values = new HashSet<RoleClaimValue?>();
                Claims.Add(claimType, values);
            }
            return values;
        }

        private HashSet<RoleClaimValue?> ClaimValues(string claimType)
            => ClaimValues(RoleClaimType(claimType));

        private RoleClaimType RoleClaimType(string claimType) => new RoleClaimType(claimType, EntityName, nameof(Claims));

        private RoleClaimType RoleClaimValue(string claimValue) => new RoleClaimType(claimValue, EntityName, nameof(Claims));
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Properties;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The role collection actor class
    /// </summary>
    public class RoleClaimsCollectionActor : ActorBase<RoleClaimsCollectionState>, IRoleClaimsCollectionActor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimsCollectionActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleClaimsCollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Create a new role claim
        /// </summary>
        /// <param name="roleClaim">The role claim</param>
        /// <exception cref="ArgumentNullException">role claim is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Role identifier not defined</exception>
        /// <exception cref="ArgumentOutOfRangeException">Claim type not defined</exception>
        public async Task Create(RoleClaim roleClaim)
        {
            if (roleClaim == null)
            {
                throw new ArgumentNullException(nameof(roleClaim));
            }
            if (roleClaim.RoleId == default)
            {
                throw new ArgumentOutOfRangeException(Resources.RoleIdNotDefined);
            }
            if (roleClaim.ClaimType == default)
            {
                throw new ArgumentOutOfRangeException(Resources.ClaimTypeNotDefined);
            }
            if (!State.RoleIds.Contains(roleClaim.RoleId))
            {
                State.RoleIds.Add(roleClaim.RoleId);
            }
            if (!State.ClaimTypes.ContainsKey(roleClaim.ClaimType))
            {
                State.ClaimTypes.Add(roleClaim.ClaimType, new HashSet<Guid>() { roleClaim.RoleId });
            }
            await IdentityActors.RoleClaims(roleClaim.RoleId).Add(roleClaim.ClaimType, roleClaim.ClaimValue);
            await SetStateData();
        }

        /// <summary>
        /// Delete a role claim
        /// </summary>
        /// <param name="roleClaim">The role claim</param>
        /// <exception cref="ArgumentNullException">role claim is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Role identifier not defined</exception>
        /// <exception cref="ArgumentOutOfRangeException">Claim type not defined</exception>
        public async Task Delete(RoleClaim roleClaim)
        {
            if (roleClaim == null)
            {
                throw new ArgumentNullException(nameof(roleClaim));
            }

            if (roleClaim.RoleId == default)
            {
                throw new ArgumentOutOfRangeException(Resources.RoleIdNotDefined);
            }

            if (roleClaim.ClaimType == default)
            {
                throw new ArgumentOutOfRangeException(Resources.ClaimTypeNotDefined);
            }
            await IdentityActors.RoleClaims(roleClaim.RoleId).Add(roleClaim.ClaimType, roleClaim.ClaimValue);
        }

        /// <summary>
        /// Gets the roles having a claim with the given type and value.
        /// </summary>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="claimValue">The claim value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Claim type</exception>
        public async Task<IList<Role>> GetRoles(string claimType, string claimValue)
        {
            if (string.IsNullOrWhiteSpace(claimType))
            {
                throw new ArgumentNullException(claimType);
            }
            var list = new List<Task<Role?>>();
            foreach (Guid roleId in State.ClaimTypes.Where(p => p.Key == claimType).SelectMany(p => p.Value))
            {
                list.Add(FindClaimRole(roleId, claimType, claimValue));
            }
            return (IList<Role>)(await Task.WhenAll(list)).Where(p => p != null).ToList();
        }

        /// <summary>
        /// Update a role claim
        /// </summary>
        /// <param name="roleClaim">The role claim</param>
        /// <exception cref="ArgumentNullException">role claim is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Role identifier not defined</exception>
        /// <exception cref="ArgumentOutOfRangeException">Claim type not defined</exception>
        public async Task Update(RoleClaim roleClaim)
        {
            if (roleClaim == null)
            {
                throw new ArgumentNullException(nameof(roleClaim));
            }

            if (roleClaim.RoleId == default)
            {
                throw new ArgumentOutOfRangeException(Resources.RoleIdNotDefined);
            }

            if (roleClaim.ClaimType == default)
            {
                throw new ArgumentOutOfRangeException(Resources.ClaimTypeNotDefined);
            }
            if (!State.RoleIds.Contains(roleClaim.RoleId))
            {
                State.RoleIds.Add(roleClaim.RoleId);
            }
            if (!State.ClaimTypes.ContainsKey(roleClaim.ClaimType))
            {
                State.ClaimTypes.Add(roleClaim.ClaimType, new HashSet<Guid>() { roleClaim.RoleId });
            }
            await IdentityActors.RoleClaims(roleClaim.RoleId).Add(roleClaim.ClaimType, roleClaim.ClaimValue);
        }

        private static async Task<Role?> FindClaimRole(Guid roleId, string claimType, string claimValue)
        {
            if (await IdentityActors.RoleClaims(roleId).Exist(claimType, claimValue))
            {
                return await IdentityActors.Role(roleId).GetRole();
            }
            return null;
        }
    }
}
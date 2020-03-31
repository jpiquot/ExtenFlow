using System;
using System.Collections.Generic;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The role claim collection state
    /// </summary>
    public class RoleClaimsCollectionState
    {
        private Dictionary<string, HashSet<Guid>>? _claimTypes;
        private HashSet<Guid>? _roleIds;

        /// <summary>
        /// The roles by claim type
        /// </summary>
        public Dictionary<string, HashSet<Guid>> ClaimTypes => _claimTypes ?? (_claimTypes = new Dictionary<string, HashSet<Guid>>());

        /// <summary>
        /// The ids of all existing roles
        /// </summary>
        public HashSet<Guid> RoleIds => _roleIds ?? (_roleIds = new HashSet<Guid>());
    }
}
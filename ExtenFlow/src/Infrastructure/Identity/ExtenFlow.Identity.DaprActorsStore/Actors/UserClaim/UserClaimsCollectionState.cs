using System;
using System.Collections.Generic;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user collection state
    /// </summary>
    public class UserClaimsCollectionState
    {
        private HashSet<Guid>? _userIds;
        private Dictionary<string, HashSet<Guid>>? _claimTypes;

        /// <summary>
        /// The ids of all existing users
        /// </summary>
        public HashSet<Guid> UserIds => _userIds ?? (_userIds = new HashSet<Guid>());

        /// <summary>
        /// The users by claim type
        /// </summary>
        public Dictionary<string, HashSet<Guid>> ClaimTypes => _claimTypes ?? (_claimTypes = new Dictionary<string, HashSet<Guid>>());
    }
}
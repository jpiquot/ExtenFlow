using System;
using System.Collections.Generic;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The user claim collection state
    /// </summary>
    public class UserClaimsCollectionState
    {
        private Dictionary<string, HashSet<Guid>>? _claimTypes;
        private HashSet<Guid>? _userIds;

        /// <summary>
        /// The users by claim type
        /// </summary>
        public Dictionary<string, HashSet<Guid>> ClaimTypes => _claimTypes ?? (_claimTypes = new Dictionary<string, HashSet<Guid>>());

        /// <summary>
        /// The ids of all existing users
        /// </summary>
        public HashSet<Guid> UserIds => _userIds ?? (_userIds = new HashSet<Guid>());
    }
}
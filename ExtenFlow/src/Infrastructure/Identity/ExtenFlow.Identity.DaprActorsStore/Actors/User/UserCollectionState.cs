using System;
using System.Collections.Generic;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user collection state
    /// </summary>
    public class UserCollectionState
    {
        private HashSet<Guid>? _ids;
        private Dictionary<string, Guid>? _normalizedNames;

        /// <summary>
        /// The ids of all existing users
        /// </summary>
        public HashSet<Guid> Ids => _ids ?? (_ids = new HashSet<Guid>());

        /// <summary>
        /// The normalized names of all existing users
        /// </summary>
        public Dictionary<string, Guid> NormalizedNames => _normalizedNames ?? (_normalizedNames = new Dictionary<string, Guid>());
    }
}
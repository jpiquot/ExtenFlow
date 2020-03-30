using System;
using System.Collections.Generic;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The role collection state
    /// </summary>
    public class RoleCollectionState
    {
        private HashSet<Guid>? _ids;

        private Dictionary<string, Guid>? _normalizedNames;

        /// <summary>
        /// Gets the all roles ids.
        /// </summary>
        /// <value>The role ids.</value>
        public HashSet<Guid> Ids => _ids ?? (_ids = new HashSet<Guid>());

        /// <summary>
        /// Gets all the role normalized names.
        /// </summary>
        /// <value>The role normalized names.</value>
        public Dictionary<string, Guid> NormalizedNames => _normalizedNames ?? (_normalizedNames = new Dictionary<string, Guid>());
    }
}
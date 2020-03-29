using System;
using System.Collections.Generic;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The role collection state
    /// </summary>
    public class RoleCollectionState
    {
        /// <summary>
        /// The ids of all existing roles
        /// </summary>
        public HashSet<Guid> Ids = new HashSet<Guid>();

        /// <summary>
        /// The normalized names of all existing roles
        /// </summary>
        public Dictionary<string, Guid> NormalizedNames = new Dictionary<string, Guid>();
    }
}
using System;
using System.Collections.Generic;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user collection state
    /// </summary>
    public class UserCollectionState
    {
        /// <summary>
        /// The ids of all existing users
        /// </summary>
        public HashSet<Guid> Ids = new HashSet<Guid>();

        /// <summary>
        /// The normalized names of all existing users
        /// </summary>
        public Dictionary<string, Guid> NormalizedNames = new Dictionary<string, Guid>();
    }
}
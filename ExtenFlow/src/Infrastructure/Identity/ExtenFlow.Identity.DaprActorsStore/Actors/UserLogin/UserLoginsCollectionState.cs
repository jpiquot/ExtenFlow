using System;
using System.Collections.Generic;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The user collection state
    /// </summary>
    public class UserLoginsCollectionState
    {
        /// <summary>
        /// The normalized names of all existing users
        /// </summary>
        public Dictionary<string, Guid> LoginProviders = new Dictionary<string, Guid>();
    }
}
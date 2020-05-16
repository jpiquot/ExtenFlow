using System.Collections.Generic;

namespace ExtenFlow.Identity.Users.Actors
{
    /// <summary>
    /// User claims actor state class.
    /// </summary>
    public class UserClaimsState
    {
        private Dictionary<string, HashSet<string>>? _claims;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimsState"/> class.
        /// </summary>
        public UserClaimsState()
        {
        }

        internal Dictionary<string, HashSet<string>> Claims => _claims ?? (_claims = new Dictionary<string, HashSet<string>>());
    }
}
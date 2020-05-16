using System.Collections.Generic;

namespace ExtenFlow.Identity.Roles.Actors
{
    /// <summary>
    /// Role claims actor state class.
    /// </summary>
    public class RoleClaimsState
    {
        private Dictionary<string, HashSet<string>>? _claims;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimsState"/> class.
        /// </summary>
        public RoleClaimsState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleClaimsState"/> class.
        /// </summary>
        public RoleClaimsState(Dictionary<string, HashSet<string>> claims)
        {
            _claims = claims;
        }

        internal Dictionary<string, HashSet<string>> Claims => _claims ?? (_claims = new Dictionary<string, HashSet<string>>());
    }
}
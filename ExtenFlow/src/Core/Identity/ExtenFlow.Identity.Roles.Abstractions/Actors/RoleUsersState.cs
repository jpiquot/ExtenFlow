using System.Collections.Generic;

namespace ExtenFlow.Identity.Roles.Actors
{
    /// <summary>
    /// Role claims actor state class.
    /// </summary>
    public class RoleUsersState
    {
        private HashSet<string>? _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleUsersState"/> class.
        /// </summary>
        public RoleUsersState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleUsersState"/> class.
        /// </summary>
        public RoleUsersState(HashSet<string> users)
        {
            _users = users;
        }

        internal HashSet<string> Users => _users ?? (_users = new HashSet<string>());
    }
}
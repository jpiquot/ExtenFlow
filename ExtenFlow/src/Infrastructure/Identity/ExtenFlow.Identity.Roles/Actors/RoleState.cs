using System;
using System.Globalization;

namespace ExtenFlow.Identity.Roles
{
    /// <summary>
    /// Class RoleActorState.
    /// </summary>
    public class RoleState
    {
        /// <summary>
        /// Gets the concurrency stamp. A random value that should change whenever a role is
        /// persisted to the store.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        public string? ConcurrencyStamp { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string? Name { get; internal set; }

        /// <summary>
        /// Gets or sets the normalized name of the role.
        /// </summary>
        /// <value>The normalized name.</value>
        public string? NormalizedName { get; internal set; }

        internal void ConcurrencyCheck(string? concurrencyStamp)
        {
            if (!string.IsNullOrWhiteSpace(ConcurrencyStamp) && concurrencyStamp != ConcurrencyStamp)
            {
                throw new RoleConcurrencyFailureException(CultureInfo.CurrentCulture, concurrencyStamp, ConcurrencyStamp);
            }
        }

        internal void InitNewConcurrencyStamp() => ConcurrencyStamp = Guid.NewGuid().ToString();
    }
}
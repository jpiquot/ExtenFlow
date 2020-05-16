using System;
using System.Globalization;
using ExtenFlow.Identity.Users.Exceptions;

namespace ExtenFlow.Identity.Users.Actors
{
    /// <summary>
    /// Class UserActorState.
    /// </summary>
    public class UserState
    {
        /// <summary>
        /// Gets the concurrency stamp. A random value that should change whenever a user is
        /// persisted to the store.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        public string? ConcurrencyStamp { get; private set; }

        /// <summary>
        /// Gets or sets the normalized name of the user.
        /// </summary>
        /// <value>The normalized name.</value>
        public string? NormalizedName { get; internal set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string? UserName { get; internal set; }

        internal void ConcurrencyCheck(string? concurrencyStamp)
        {
            if (!string.IsNullOrWhiteSpace(ConcurrencyStamp) && concurrencyStamp != ConcurrencyStamp)
            {
                throw new UserConcurrencyFailureException(CultureInfo.CurrentCulture, concurrencyStamp, ConcurrencyStamp);
            }
        }

        internal void InitNewConcurrencyStamp() => ConcurrencyStamp = Guid.NewGuid().ToString();
    }
}
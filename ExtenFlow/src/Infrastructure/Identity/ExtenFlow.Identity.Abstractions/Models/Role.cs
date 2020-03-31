using System;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Models
{
    /// <summary>
    /// The user class
    /// </summary>
    public class Role : IdentityRole<Guid>
    {
        /// <summary>
        /// Copies the specified role property values to this role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <exception cref="ArgumentNullException">role</exception>
        public void Copy(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            ConcurrencyStamp = role.ConcurrencyStamp;
            Id = role.Id;
            Name = role.Name;
            NormalizedName = role.NormalizedName;
        }
    }
}
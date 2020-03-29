using System;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Models
{
    /// <summary>
    /// The user class
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <remarks>The Id property is initialized to form a new GUID string value.</remarks>
        public User()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        public User(User user)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));
            Copy(user);
        }

        /// <summary>
        /// Copies the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        public void Copy(User user)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));
            Id = user.Id;
            LockoutEnabled = user.LockoutEnabled;
            LockoutEnd = user.LockoutEnd;
            NormalizedEmail = user.NormalizedEmail;
            NormalizedUserName = user.NormalizedUserName;
            PasswordHash = user.PasswordHash;
            PhoneNumber = user.PhoneNumber;
            PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            SecurityStamp = user.SecurityStamp;
            TwoFactorEnabled = user.TwoFactorEnabled;
            UserName = user.UserName;
        }
    }
}
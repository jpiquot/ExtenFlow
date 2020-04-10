using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json;

namespace ExtenFlow.Infrastructure
{
    /// <summary>
    /// Email value object
    /// </summary>
    /// <seealso cref="ExtenFlow.Infrastructure.ValueObject"/>
    [DebuggerDisplay("{UserName}@{DomainName}")]
    public class Email : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class.
        /// </summary>
        [Obsolete("Can only be used by serializers")]
        public Email()
        {
            DomainName = string.Empty;
            UserName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        public Email(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            string[] parts = email.Split('@');
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
            {
                // The email should have two parts, seperated by '@'. Email='{0}'.
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.EmailShouldHaveTwoParts, email), nameof(email));
            }
            UserName = parts[0];
            DomainName = parts[1];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domainName">Name of the domain.</param>
        [JsonConstructor]
        public Email(string userName, string domainName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                // The email user name is not defined.
                throw new ArgumentException(Properties.Resources.EmailUndefinedUserName, nameof(userName));
            }
            if (string.IsNullOrWhiteSpace(domainName))
            {
                // The email domain name is not defined.
                throw new ArgumentException(Properties.Resources.EmailUndefinedUserName, nameof(domainName));
            }
            UserName = userName;
            DomainName = domainName;
        }

        /// <summary>
        /// Gets the name of the domain.
        /// </summary>
        /// <value>The name of the domain.</value>
        public string DomainName { get; private set; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; private set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return UserName + "@" + DomainName;
        }

        /// <summary>
        /// Gets the equality components.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserName;
            yield return DomainName;
        }
    }
}
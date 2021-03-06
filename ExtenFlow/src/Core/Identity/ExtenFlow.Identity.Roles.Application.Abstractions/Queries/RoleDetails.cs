﻿using System;

namespace ExtenFlow.Identity.Roles.Application.Queries
{
    /// <summary>
    /// Role Details view model class.
    /// </summary>
    public class RoleDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDetails"/> class.
        /// </summary>
        [Obsolete("Can only be used by serializers")]
        public RoleDetails()
        {
            Name = NormalizedName = ConcurrencyStamp = string.Empty;
            Id = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDetails"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="normalizedName">Name of the normalized.</param>
        /// <param name="concurrencyStamp">The concurrency stamp.</param>
        /// <exception cref="ArgumentException">Id is not defined</exception>
        /// <exception cref="ArgumentNullException">name</exception>
        /// <exception cref="ArgumentNullException">normalizedName</exception>
        public RoleDetails(string id, string name, string normalizedName, string? concurrencyStamp)
        {
            if (id == default)
            {
                throw new ArgumentException(Domain.Properties.Resources.RoleIdNotDefined, nameof(id));
            }
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            NormalizedName = normalizedName ?? throw new ArgumentNullException(nameof(normalizedName));
            ConcurrencyStamp = concurrencyStamp;
        }

        /// <summary>
        /// Gets the concurrency stamp.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        public string? ConcurrencyStamp { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the normalized name.
        /// </summary>
        /// <value>The name of the normalized.</value>
        public string NormalizedName { get; }
    }
}
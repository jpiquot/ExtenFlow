// *********************************************************************** Assembly :
// ExtenFlow.Identity.Roles.Abstractions Author : jpiquot Created : 04-12-2020
//
// Last Modified By : jpiquot Last Modified On : 04-12-2020 ***********************************************************************
// <copyright file="RoleDetailsViewModel.cs" company="Fiveforty">
//     Copyright (c) Fiveforty Corporation. All rights reserved.
// </copyright>
// <summary>
// </summary>
// ***********************************************************************
using System;

namespace ExtenFlow.Identity.Roles.Queries
{
    /// <summary>
    /// Role Details view model class.
    /// </summary>
    public class RoleDetailsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDetailsModel"/> class.
        /// </summary>
        [Obsolete("Can only be used by serializers")]
        public RoleDetailsModel()
        {
            Name = NormalizedName = ConcurrencyStamp = string.Empty;
            Id = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDetailsModel"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="normalizedName">Name of the normalized.</param>
        /// <param name="concurrencyStamp">The concurrency stamp.</param>
        /// <exception cref="ArgumentException">Id is not defined</exception>
        /// <exception cref="ArgumentNullException">name</exception>
        /// <exception cref="ArgumentNullException">normalizedName</exception>
        public RoleDetailsModel(string id, string name, string normalizedName, string? concurrencyStamp)
        {
            if (id == default)
            {
                throw new ArgumentException(Properties.Resources.RoleIdNotDefined, nameof(id));
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
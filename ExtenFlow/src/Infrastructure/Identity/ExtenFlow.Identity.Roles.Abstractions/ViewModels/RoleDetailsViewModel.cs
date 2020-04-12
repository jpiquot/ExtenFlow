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

using ExtenFlow.Identity.Models;

namespace ExtenFlow.Identity.Roles
{
    /// <summary>
    /// Role Details view model class.
    /// </summary>
    public class RoleDetailsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDetailsViewModel"/> class.
        /// </summary>
        [Obsolete("Can only be used by serializers")]
        public RoleDetailsViewModel()
        {
            Name = NormalizedName = ConcurrencyStamp = string.Empty;
            Id = Guid.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleDetailsViewModel"/> class.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <exception cref="ArgumentNullException">role</exception>
        public RoleDetailsViewModel(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            Id = role.Id;
            Name = role.Name;
            NormalizedName = role.NormalizedName;
            ConcurrencyStamp = role.ConcurrencyStamp;
        }

        /// <summary>
        /// Gets the concurrency stamp.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        public string ConcurrencyStamp { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; }

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
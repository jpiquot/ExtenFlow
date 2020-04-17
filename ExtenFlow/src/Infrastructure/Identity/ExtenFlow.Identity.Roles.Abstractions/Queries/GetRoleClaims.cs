// *********************************************************************** Assembly :
// ExtenFlow.Identity.Roles.Abstractions Author : jpiquot Created : 04-12-2020
//
// Last Modified By : jpiquot Last Modified On : 04-12-2020 ***********************************************************************
// <copyright file="GetRoleClaims.cs" company="Fiveforty">
//     Copyright (c) Fiveforty Corporation. All rights reserved.
// </copyright>
// <summary>
// </summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ExtenFlow.Identity.Roles.Queries
{
    /// <summary>
    /// Class GetRoleClaims. Implements the <see cref="RoleQuery{RoleDetailsViewModel}"/>
    /// </summary>
    /// <seealso cref="RoleQuery{RoleDetailsViewModel}"/>
    public class GetRoleClaims : RoleQuery<IList<Claim>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleClaims"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public GetRoleClaims()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleClaims"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="messageId">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public GetRoleClaims(string aggregateId, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
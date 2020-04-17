// *********************************************************************** Assembly :
// ExtenFlow.Identity.Roles.Abstractions Author : jpiquot Created : 04-12-2020
//
// Last Modified By : jpiquot Last Modified On : 04-12-2020 ***********************************************************************
// <copyright file="GetRoleDetails.cs" company="Fiveforty">
//     Copyright (c) Fiveforty Corporation. All rights reserved.
// </copyright>
// <summary>
// </summary>
// ***********************************************************************
using System;

namespace ExtenFlow.Identity.Roles.Queries
{
    /// <summary>
    /// Class GetRoleDetails. Implements the <see cref="RoleQuery{RoleDetailsViewModel}"/>
    /// </summary>
    /// <seealso cref="RoleQuery{RoleDetailsViewModel}"/>
    public class GetRoleDetails : RoleQuery<RoleDetailsModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleDetails"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public GetRoleDetails()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRoleDetails"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="messageId">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public GetRoleDetails(string aggregateId, string userId, Guid? correlationId = null, Guid? messageId = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), messageId ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
// *********************************************************************** Assembly :
// ExtenFlow.Identity.Users.Abstractions Author : jpiquot Created : 04-12-2020
//
// Last Modified By : jpiquot Last Modified On : 04-12-2020 ***********************************************************************
// <copyright file="GetUserDetails.cs" company="Fiveforty">
//     Copyright (c) Fiveforty Corporation. All rights reserved.
// </copyright>
// <summary>
// </summary>
// ***********************************************************************
using System;

namespace ExtenFlow.Identity.Users.Queries
{
    /// <summary>
    /// Class GetUserDetails. Implements the <see cref="UserQuery{UserDetailsViewModel}"/>
    /// </summary>
    /// <seealso cref="UserQuery{UserDetailsViewModel}"/>
    public class GetUserDetails : UserQuery<UserDetailsModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserDetails"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public GetUserDetails()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserDetails"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public GetUserDetails(string aggregateId, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base(aggregateId, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
// *********************************************************************** Assembly :
// ExtenFlow.Identity.Users.Abstractions Author : jpiquot Created : 04-12-2020
//
// Last Modified By : jpiquot Last Modified On : 04-12-2020 ***********************************************************************
// <copyright file="UserQuery.cs" company="Fiveforty">
//     Copyright (c) Fiveforty Corporation. All rights reserved.
// </copyright>
// <summary>
// </summary>
// ***********************************************************************
using System;

using ExtenFlow.Identity.Models;
using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Users.Queries
{
    /// <summary>
    /// Class UserQuery. Implements the <see cref="ExtenFlow.Messages.Query{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ExtenFlow.Messages.Query{T}"/>
    public class UserQuery<T> : Query<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserQuery{T}"/> class.
        /// </summary>
        /// <remarks>Do not use this constructor. It has been added for serializers</remarks>
        [Obsolete("Can only be used by serializers")]
        public UserQuery()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserQuery{T}"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="userId">The identifier of the user that created the message.</param>
        /// <param name="correlationId">The correlation identifier. Used to link messages together.</param>
        /// <param name="id">The message unique identifier.</param>
        /// <param name="dateTime">The date time, the message was created.</param>
        public UserQuery(string aggregateId, string userId, Guid? correlationId = null, Guid? id = null, DateTimeOffset? dateTime = null)
            : base(nameof(User), aggregateId, userId, correlationId ?? Guid.NewGuid(), id ?? Guid.NewGuid(), dateTime ?? DateTimeOffset.Now)
        {
        }
    }
}
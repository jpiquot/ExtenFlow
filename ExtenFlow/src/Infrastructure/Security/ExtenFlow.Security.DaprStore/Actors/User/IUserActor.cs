﻿using System.Threading.Tasks;

using Dapr.Actors;

using ExtenFlow.Security.Users;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Security.DaprStore.Actors
{
    /// <summary>
    /// The user actor
    /// </summary>
    public interface IUserActor : IActor
    {
        /// <summary>
        /// Find user id
        /// </summary>
        /// <returns>Returns the user id or null if it does not exist</returns>
        Task<string> FindUserId();

        /// <summary>
        /// Get the user name
        /// </summary>
        /// <returns>The user name</returns>
        Task<string> GetUserName();

        /// <summary>
        /// Set the user name
        /// </summary>
        /// <param name="userName">The new user name</param>
        Task SetUserName(string userName);

        /// <summary>
        /// Get the user normalized name
        /// </summary>
        /// <returns>The user normalized name</returns>
        Task<string> GetNormalizedUserName();

        /// <summary>
        /// Set the user normalized name
        /// </summary>
        /// <param name="normalizedName">The new normalized name</param>
        /// <returns></returns>
        Task SetNormalizedUserName(string normalizedName);

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Create(User user);

        /// <summary>
        /// Update user properties
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Update(User user);

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <returns>The operation result</returns>
        Task<IdentityResult> Delete();

        /// <summary>
        /// Getthe user
        /// </summary>
        /// <returns>The user object</returns>
        Task<User> GetUser();
    }
}
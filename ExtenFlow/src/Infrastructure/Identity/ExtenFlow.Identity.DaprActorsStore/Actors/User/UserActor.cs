using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The User Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IUserActor"/>
    public class UserActor : BaseActor<User>, IUserActor
    {
        private readonly IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Update user properties
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        /// <exception cref="InvalidOperationException">
        /// The user Id ({user?.Id}) is not the same as the actor Id({Id.GetId()})
        /// </exception>
        public async Task<IdentityResult> SetUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.Id == default)
            {
                throw new ArgumentOutOfRangeException(Resource.UserIdNotDefined);
            }
            if (_state != null && user.ConcurrencyStamp != State.ConcurrencyStamp)
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            await SetState(user);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Clear the user
        /// </summary>
        /// <returns>The operation result</returns>
        /// <exception cref="KeyNotFoundException">The user Id ({Id.GetId()}) already exist.</exception>
        public async Task<IdentityResult> ClearUser(string concurrencyStamp)
        {
            if (_state == null)
            {
                throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resource.UserNotFound, Id.GetId()));
            }
            if (State.ConcurrencyStamp != concurrencyStamp)
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            await SetState(null);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Getthe user
        /// </summary>
        /// <returns>The user object</returns>
        public Task<User> GetUser() => _state == null ? Task.FromException<User>(new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resource.UserNotFound, Id.GetId()))) : Task.FromResult(State);
    }
}
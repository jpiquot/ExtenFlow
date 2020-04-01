using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

using ExtenFlow.Identity.Properties;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The user collection actor class
    /// </summary>
    public class UserCollectionActor : BaseActor<UserCollectionState>, IUserCollectionActor
    {
        private readonly IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCollectionActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public UserCollectionActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Create(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.Id == default)
            {
                throw new ArgumentOutOfRangeException(Resources.UserIdNotDefined);
            }
            if (State.Ids.Any(p => p == user.Id))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateUser, Id.GetId()));
            }
            if (State.NormalizedNames.Any(p => p.Key == user.NormalizedUserName))
            {
                return IdentityResult.Failed(_errorDescriber.DuplicateUserName(user.NormalizedUserName));
            }
            IdentityResult result = await IdentityActors.User(user.Id).SetUser(user);
            if (result.Succeeded)
            {
                State.Ids.Add(user.Id);
                State.NormalizedNames.Add(user.NormalizedUserName, user.Id);
                await SetState();
            }
            return result;
        }

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Delete(Guid userId, string concurrencyString)
        {
            if (userId == default)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (!State.Ids.Any(p => p == userId))
            {
                throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.UserNotFound, Id.GetId()));
            }
            State.NormalizedNames.Remove(State.NormalizedNames.Where(p => p.Value == userId).Select(p => p.Key).Single());
            State.Ids.Remove(userId);
            await SetState();
            await IdentityActors.User(userId).ClearUser(concurrencyString);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Checks if a user with the given identifier exists.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>true if the user exists, else false.</returns>
        public Task<bool> Exist(Guid userId)
        {
            if (userId == default)
            {
                return Task.FromException<bool>(new ArgumentNullException(nameof(userId)));
            }
            return Task.FromResult(State.Ids.Any(p => p == userId));
        }

        /// <summary>
        /// Finds the user by it's normalized name.
        /// </summary>
        /// <param name="normalizedUserName">The user normalized name.</param>
        /// <returns>The user id if exists, else null.</returns>
        public Task<Guid?> FindByNormalizedName(string normalizedUserName)
        {
            if (string.IsNullOrWhiteSpace(normalizedUserName))
            {
                Task.FromException<Guid?>(new ArgumentNullException(nameof(normalizedUserName)));
            }
            return Task.FromResult<Guid?>(State.NormalizedNames.Where(p => p.Key == normalizedUserName).Select(p => p.Value).FirstOrDefault());
        }

        /// <summary>
        /// Gets the all the user ids.
        /// </summary>
        /// <returns>The user ids</returns>
        public Task<IList<Guid>> GetIds()
            => Task.FromResult<IList<Guid>>(State.Ids.ToList());

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">The new user properties</param>
        /// <returns>The operation result</returns>
        public async Task<IdentityResult> Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.Id == default)
            {
                throw new ArgumentOutOfRangeException(Resources.UserIdNotDefined);
            }
            if (!State.Ids.Any(p => p == user.Id))
            {
                throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.UserNotFound, Id.GetId()));
            }
            if (State.NormalizedNames.Any(p => p.Key == user.NormalizedUserName && p.Value != user.Id))
            {
                return IdentityResult.Failed(_errorDescriber.DuplicateUserName(user.NormalizedUserName));
            }
            IdentityResult result = await IdentityActors.User(user.Id).SetUser(user);
            if (result.Succeeded)
            {
                if (!State.NormalizedNames.Any(p => p.Key == user.NormalizedUserName))
                {
                    // The normalized name hase been changed.
                    State.NormalizedNames.Remove(State.NormalizedNames.Where(p => p.Value == user.Id).Select(p => p.Key).Single());
                    State.NormalizedNames.Add(user.NormalizedUserName, user.Id);
                }
                await SetState();
            }
            return result;
        }
    }
}
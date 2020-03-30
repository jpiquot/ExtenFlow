using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Identity.Models;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.DaprActorsStore
{
    /// <summary>
    /// The User Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IRoleActor"/>
    public class RoleActor : BaseActor<Role>, IRoleActor
    {
        private IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="P:Dapr.Actors.Runtime.Actor.ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleActor(ActorService actorService, ActorId actorId, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <returns>The role object</returns>
        public Task<Role> Get() => Task.FromResult(State);

        /// <summary>
        /// Updates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <exception cref="ArgumentNullException">Role.Id</exception>
        /// <returns>The identity result object</returns>
        public async Task<IdentityResult> Set(Role role)
        {
            if (role == null || role.Id == default)
            {
                throw new ArgumentNullException(nameof(Role) + "." + nameof(Role.Id));
            }
            if (_state != null && !role.ConcurrencyStamp.Equals(State.ConcurrencyStamp))
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            await SetState(role);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Clears the specified concurrency string.
        /// </summary>
        /// <param name="concurrencyString">The concurrency string.</param>
        /// <returns>The identity result object</returns>
        public async Task<IdentityResult> Clear(string concurrencyString)
        {
            if (_state == null)
            {
                throw new KeyNotFoundException("The role does not exist.");
            }
            if (!State.ConcurrencyStamp.Equals(concurrencyString))
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            await SetState(null);
            return IdentityResult.Success;
        }
    }
}
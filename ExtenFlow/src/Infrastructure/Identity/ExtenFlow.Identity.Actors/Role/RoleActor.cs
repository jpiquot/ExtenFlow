using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using ExtenFlow.Actors;
using ExtenFlow.Identity.Models;
using ExtenFlow.Identity.Properties;

using Microsoft.AspNetCore.Identity;

namespace ExtenFlow.Identity.Actors
{
    /// <summary>
    /// The User Actor class
    /// </summary>
    /// <seealso cref="Actor"/>
    /// <seealso cref="IRoleActor"/>
    public class RoleActor : ActorBase<Role>, IRoleActor
    {
        private readonly IIndexService _indexService;
        private IdentityErrorDescriber _errorDescriber = new IdentityErrorDescriber();

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleActor"/> class.
        /// </summary>
        /// <param name="actorService">
        /// The <see cref="ActorService"/> that will host this actor instance.
        /// </param>
        /// <param name="actorId">The Id of the actor.</param>
        /// <param name="indexService"></param>
        /// <param name="actorStateManager">The custom implementation of the StateManager.</param>
        public RoleActor(ActorService actorService, ActorId actorId, IIndexService indexService, IActorStateManager? actorStateManager = null) : base(actorService, actorId, actorStateManager)
        {
            _indexService = indexService;
        }

        /// <summary>
        /// Clears the role.
        /// </summary>
        /// <param name="concurrencyString">The concurrency string.</param>
        /// <returns>The identity result object</returns>
        public async Task<IdentityResult> Clear(string concurrencyString)
        {
            if (State == null || State.Id == default)
            {
                throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.RoleNotFound, Id.GetId()));
            }
            if (State.ConcurrencyStamp != concurrencyString)
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            await _indexService.Remove(Id.GetId());
            State = null;
            await SetStateData();
            return IdentityResult.Success;
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <returns>The role object</returns>
        public Task<Role> GetRole()
        {
            if (State == null || State.Id == default)
            {
                return Task.FromException<Role>(new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.RoleNotFound, Id.GetId())));
            }
            return Task.FromResult<Role>(State);
        }

        /// <summary>
        /// Updates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <exception cref="ArgumentNullException">Role.Id</exception>
        /// <returns>The identity result object</returns>
        public async Task<IdentityResult> SetRole(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (role.Id == default)
            {
                throw new ArgumentOutOfRangeException(Resources.RoleIdNotDefined);
            }
            if (State?.ConcurrencyStamp != null && role.ConcurrencyStamp != State.ConcurrencyStamp)
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            await SetStateData();
            return IdentityResult.Success;
        }
    }
}
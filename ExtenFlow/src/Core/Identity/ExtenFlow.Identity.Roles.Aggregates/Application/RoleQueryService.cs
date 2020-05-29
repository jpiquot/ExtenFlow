using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using ExtenFlow.Identity.Roles.Actors;
using ExtenFlow.Identity.Roles.Queries;
using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Roles.Application
{
    /// <summary>
    /// Class RoleQueryService. Implements the <see cref="ExtenFlow.Identity.Roles.Application.IRoleConsistentQueryService"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Roles.Application.IRoleConsistentQueryService"/>
    public sealed class RoleQueryService : IRoleConsistentQueryService
    {
        private readonly Func<string, IRoleActor> _getRoleActor;
        private readonly Func<string, IRoleNameRegistryEntryActor> _getRoleNameRegistryEntryActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleQueryService"/> class.
        /// </summary>
        /// <param name="getRoleActor">The get role actor.</param>
        /// <param name="getRoleNameRegistryEntryActor">The get role name registry entry actor.</param>
        public RoleQueryService(
            Func<string, IRoleActor> getRoleActor,
            Func<string, IRoleNameRegistryEntryActor> getRoleNameRegistryEntryActor)
        {
            _getRoleActor = getRoleActor;
            _getRoleNameRegistryEntryActor = getRoleNameRegistryEntryActor;
        }

        /// <summary>
        /// Asks the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;IList&lt;Claim&gt;&gt;.</returns>
        public Task<IList<Claim>> Ask(GetRoleClaims query)
            => AskRole<GetRoleClaimsValidator, IList<Claim>>(query);

        /// <summary>
        /// Asks the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;RoleDetails&gt;.</returns>
        public Task<RoleDetails> Ask(GetRoleDetails query)
            => AskRole<GetRoleDetailsValidator, RoleDetails>(query);

        /// <summary>
        /// Asks the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
        public Task<string> Ask(GetRoleIdByName query)
            => AskRoleNameRegistry<GetRoleIdByNameValidator, string>(query);

        /// <summary>
        /// Asks the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.

        public Task<string> Ask(FindRoleIdByName query)
            => AskRoleNameRegistry<FindRoleIdByNameValidator, string?>(query);

#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

        /// <summary>
        /// Asks the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> Ask(IsRoleNameRegistered query)
            => AskRoleNameRegistry<IsRoleNameRegisteredValidator, bool>(query);

        private async Task<TResult> AskRole<TValidator, TResult>(RoleQuery<TResult>? query)
            where TValidator : RoleQueryValidator<TResult>, new()
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));
            IRoleActor actor = _getRoleActor(query.AggregateId ?? query.AggregateType);
            return (TResult)(await actor.Ask(new Envelope(query)));
        }

        private async Task<TResult> AskRoleNameRegistry<TValidator, TResult>(RoleNameRegistryQuery<TResult>? query)
            where TValidator : RoleNameRegistryQueryValidator<TResult>, new()
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));
            IRoleNameRegistryEntryActor actor = _getRoleNameRegistryEntryActor(query.AggregateId ?? query.AggregateType);
            return (TResult)(await actor.Ask(new Envelope(query)));
        }
    }
}
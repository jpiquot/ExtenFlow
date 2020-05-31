using System;
using System.Threading.Tasks;

using ExtenFlow.Identity.Roles.Actors;
using ExtenFlow.Identity.Roles.Commands;
using ExtenFlow.Messages;

namespace ExtenFlow.Identity.Roles.Application
{
    /// <summary>
    /// Class RoleCommandService. Implements the <see cref="ExtenFlow.Identity.Roles.Application.IRoleCommandService"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Identity.Roles.Application.IRoleCommandService"/>
    public class RoleCommandService : IRoleCommandService
    {
        private readonly Func<string, IRoleActor> _getRoleActor;
        private readonly Func<string, IRoleNameRegistryEntryActor> _getRoleNameRegistryEntryActor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleCommandService"/> class.
        /// </summary>
        /// <param name="getRoleActor">The get role actor.</param>
        /// <param name="getRoleNameRegistryEntryActor">The get role name registry entry actor.</param>
        public RoleCommandService(
            Func<string, IRoleActor> getRoleActor,
            Func<string, IRoleNameRegistryEntryActor> getRoleNameRegistryEntryActor)
        {
            _getRoleActor = getRoleActor;
            _getRoleNameRegistryEntryActor = getRoleNameRegistryEntryActor;
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Handle(AddNewRole command)
            => HandleRole<AddNewRoleValidator>(command);

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Handle(AddRoleClaim command)
            => HandleRole<AddRoleClaimValidator>(command);

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Handle(AddUserToRole command)
            => HandleRole<AddUserToRoleValidator>(command);

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Handle(RegisterNormalizedRoleName command)
            => HandleRoleNameRegistry<RegisterNormalizedRoleNameValidator>(command);

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Handle(DeregisterNormalizedRoleName command)
            => HandleRoleNameRegistry<DeregisterNormalizedRoleNameValidator>(command);

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Handle(RemoveRole command)
            => HandleRole<RemoveRoleValidator>(command);

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Handle(RemoveRoleClaim command)
            => HandleRole<RemoveRoleClaimValidator>(command);

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Handle(RemoveUserFromRole command)
            => HandleRole<RenameRoleValidator>(command);

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Handle(RenameRole command)
            => HandleRole<RenameRoleValidator>(command);

        /// <summary>
        /// Handles the role.
        /// </summary>
        /// <typeparam name="TValidator">The type of the t validator.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ArgumentNullException">command</exception>
        private Task HandleRole<TValidator>(RoleCommand? command)
            where TValidator : RoleCommandValidator, new()
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            IRoleActor actor = _getRoleActor(command.AggregateId ?? command.AggregateType);
            return actor.Tell(new Envelope(command));
        }

        /// <summary>
        /// Handles the role name registry.
        /// </summary>
        /// <typeparam name="TValidator">The type of the t validator.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ArgumentNullException">command</exception>
        private Task HandleRoleNameRegistry<TValidator>(RoleNameRegistryCommand? command)
            where TValidator : RoleNameRegistryCommandValidator, new()
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            IRoleNameRegistryEntryActor actor = _getRoleNameRegistryEntryActor(command.AggregateId ?? command.AggregateType);
            return actor.Tell(new Envelope(command));
        }
    }
}
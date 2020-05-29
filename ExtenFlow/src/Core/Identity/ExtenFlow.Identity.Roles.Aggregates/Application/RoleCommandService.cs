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
        /// Tells the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Tell(AddNewRole command)
            => TellRole<AddNewRoleValidator>(command);

        /// <summary>
        /// Tells the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Tell(AddRoleClaim command)
            => TellRole<AddRoleClaimValidator>(command);

        /// <summary>
        /// Tells the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Tell(AddUserToRole command)
            => TellRole<AddUserToRoleValidator>(command);

        /// <summary>
        /// Tells the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Tell(RegisterNormalizedRoleName command)
            => TellRoleNameRegistry<RegisterNormalizedRoleNameValidator>(command);

        /// <summary>
        /// Tells the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Tell(DeregisterNormalizedRoleName command)
            => TellRoleNameRegistry<DeregisterNormalizedRoleNameValidator>(command);

        /// <summary>
        /// Tells the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Tell(RemoveRole command)
            => TellRole<RemoveRoleValidator>(command);

        /// <summary>
        /// Tells the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Tell(RemoveRoleClaim command)
            => TellRole<RemoveRoleClaimValidator>(command);

        /// <summary>
        /// Tells the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Tell(RemoveUserFromRole command)
            => TellRole<RenameRoleValidator>(command);

        /// <summary>
        /// Tells the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task Tell(RenameRole command)
            => TellRole<RenameRoleValidator>(command);

        /// <summary>
        /// Tells the role.
        /// </summary>
        /// <typeparam name="TValidator">The type of the t validator.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ArgumentNullException">command</exception>
        private Task TellRole<TValidator>(RoleCommand? command)
            where TValidator : RoleCommandValidator, new()
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            IRoleActor actor = _getRoleActor(command.AggregateId ?? command.AggregateType);
            return actor.Tell(new Envelope(command));
        }

        /// <summary>
        /// Tells the role name registry.
        /// </summary>
        /// <typeparam name="TValidator">The type of the t validator.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ArgumentNullException">command</exception>
        private Task TellRoleNameRegistry<TValidator>(RoleNameRegistryCommand? command)
            where TValidator : RoleNameRegistryCommandValidator, new()
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            IRoleNameRegistryEntryActor actor = _getRoleNameRegistryEntryActor(command.AggregateId ?? command.AggregateType);
            return actor.Tell(new Envelope(command));
        }
    }
}
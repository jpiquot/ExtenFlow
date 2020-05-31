using System;
using System.Threading;
using System.Threading.Tasks;

using ExtenFlow.Actors;
using ExtenFlow.Messages.Brighter.Requests;

using Paramore.Brighter;

namespace ExtenFlow.Messages.Brighter
{
    /// <summary>
    /// Class ActorCommandHandler. Implements the <see cref="Paramore.Brighter.RequestHandlerAsync{TCommand}"/>
    /// </summary>
    /// <typeparam name="TActor">The type of the t actor.</typeparam>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    /// <seealso cref="Paramore.Brighter.RequestHandlerAsync{TCommand}"/>
    public class ActorCommandHandler<TActor, TCommand> : RequestHandlerAsync<BrighterCommand>
        where TCommand : class, ExtenFlow.Messages.ICommand
        where TActor : IDispatchActor
    {
        /// <summary>
        /// handle as an asynchronous operation.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>TCommand.</returns>
        public override async Task<BrighterCommand> HandleAsync(BrighterCommand command, CancellationToken cancellationToken = default)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            await GetActor(command.Command.AggregateId).Tell(command.Command);
            return await base.HandleAsync(command, cancellationToken);
        }

        /// <summary>
        /// Gets the actor.
        /// </summary>
        /// <param name="id">The actor instance identifier.</param>
        /// <returns>TActor.</returns>
        protected TActor GetActor(string id) => new ActorSystem().Create<TActor>(id);
    }
}
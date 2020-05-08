using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using ExtenFlow.Actors;

using Paramore.Brighter;

namespace ExtenFlow.Messages.BrighterBroker
{
    /// <summary>
    /// Class DaprActorRequestHandler. Implements the <see cref="Paramore.Brighter.RequestHandlerAsync{TRequest}"/>
    /// </summary>
    /// <typeparam name="TCommand">The type of the t request.</typeparam>
    /// <typeparam name="TActor">The type of the t actor.</typeparam>
    /// <seealso cref="Paramore.Brighter.RequestHandlerAsync{TRequest}"/>
    public class DaprActorCommandHandler<TCommand, TActor> : RequestHandlerAsync<TCommand>
        where TCommand : class, ICommand
        where TActor : IDispatchActor
    {
        private readonly IActorSystem _actorSystem;
        private readonly int _maxHandlers;
        private IDispatchActor? _actor;
        private Random _random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="DaprActorCommandHandler{TRequest,
        /// TActor}"/> class.
        /// </summary>
        /// <param name="actorSystem"></param>
        /// <param name="maxHandlers">The maximum handlers.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public DaprActorCommandHandler(IActorSystem actorSystem, int maxHandlers = 100)
        {
            _actorSystem = actorSystem ?? throw new ArgumentNullException(nameof(actorSystem));
            if (maxHandlers < 1)
            {
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.MinimimValueForArgument, 1, nameof(maxHandlers)));
            }
            _maxHandlers = maxHandlers;
        }

        /// <summary>
        /// Gets the Actor instance
        /// </summary>
        /// <value>The actor.</value>
        public IDispatchActor Actor
            => _actor ??
                (_actor = _actorSystem
                        .Create<TActor, string>(_random.Next(1, _maxHandlers).ToString(CultureInfo.InvariantCulture)
                    ));

        /// <summary>
        /// Handles the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;TRequest&gt;.</returns>
        public override async Task<TCommand> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            await Actor.Tell(command);
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
using System;
using System.Globalization;

using Dapr.Actors;
using Dapr.Actors.Client;

using Paramore.Brighter;

namespace ExtenFlow.Messages.BrighterBroker
{
    /// <summary>
    /// Use Dapr actors for command handlers
    /// </summary>
    /// <seealso cref="Paramore.Brighter.IAmAHandlerFactoryAsync"/>
    public class DaprActorHandlerFactory : IAmAHandlerFactoryAsync
    {
        private readonly IActorProxy _actorProxy;
        private readonly int _maxHandlers;
        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="DaprActorHandlerFactory"/> class.
        /// </summary>
        /// <param name="actorProxy">Actor proxy instance</param>
        /// <param name="maxHandlers">The maximum handlers.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public DaprActorHandlerFactory(IActorProxy actorProxy, int maxHandlers = 100)
        {
            _random = new Random();
            _actorProxy = actorProxy;
            _maxHandlers = maxHandlers;
            if (maxHandlers < 1)
            {
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.MinimimValueForArgument, 1, nameof(maxHandlers)));
            }
        }

        /// <summary>
        /// Creates the specified handler type.
        /// </summary>
        /// <param name="handlerType">Type of the handler.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">handlerType</exception>
        public IHandleRequestsAsync Create(Type handlerType)
        {
            if (handlerType == null)
            {
                throw new ArgumentNullException(nameof(handlerType));
            }
            // TODO: Change create signature
            return (IHandleRequestsAsync)_actorProxy.Create(new ActorId(_random.Next(1, _maxHandlers).ToString(CultureInfo.InvariantCulture)), handlerType, handlerType.Name.Substring(1));
        }

        /// <summary>
        /// Releases the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void Release(IHandleRequestsAsync handler)
        {
        }
    }
}
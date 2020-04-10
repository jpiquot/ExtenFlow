using System;
using System.Globalization;
using System.Reflection;

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
        private readonly int _maxHandlers;
        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="DaprActorHandlerFactory"/> class.
        /// </summary>
        /// <param name="maxHandlers">The maximum handlers.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public DaprActorHandlerFactory(int maxHandlers = 100)
        {
            _random = new Random();
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
            MethodInfo? method = typeof(ActorProxy).GetMethod("Create", BindingFlags.Static);
            if (method == null)
            {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new Exception("Static method create not found on ActorProxy class.");
            }
            MethodInfo generic = method.MakeGenericMethod(handlerType);
            var handler = (IHandleRequestsAsync?)generic.Invoke(this, new object[] { new ActorId(_random.Next(1, _maxHandlers).ToString(CultureInfo.InvariantCulture)), handlerType, handlerType.Name.Substring(1) });
            if (handler == null)
            {
                throw new Exception("The actor proxy create method returned a null value.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            }
            // TODO: Change create signature
            //return (IHandleRequestsAsync)_actorProxy.Create(new ActorId(_random.Next(1, _maxHandlers).ToString(CultureInfo.InvariantCulture)), handlerType, handlerType.Name.Substring(1));
            return handler;
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
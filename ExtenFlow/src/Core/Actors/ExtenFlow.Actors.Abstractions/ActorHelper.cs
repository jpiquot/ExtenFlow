using System;
using System.Globalization;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using ExtenFlow.Messages;

namespace ExtenFlow.Actors
{
    /// <summary>
    /// Helper methods for actors
    /// </summary>
    public static class ActorHelper
    {
        /// <summary>
        /// Actor name.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>The type name without the 'Actor' part.</returns>
        /// <exception cref="System.ArgumentNullException">actor</exception>
        public static string ActorName(this Actor actor)
        {
            _ = actor ?? throw new ArgumentNullException(nameof(actor));
            return ActorName(actor.GetType());
        }

        /// <summary>
        /// Actors the name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.String.</returns>
        public static string ActorName<T>() => ActorName(typeof(T));

        /// <summary>
        /// The actors type name.
        /// </summary>
        /// <param name="actorType">Type of the actor.</param>
        /// <returns>The type name without the 'Actor' part.</returns>
        /// <exception cref="ArgumentNullException">actorType</exception>
        public static string ActorName(Type actorType)
        {
            _ = actorType ?? throw new ArgumentNullException(nameof(actorType));
            return ActorName(actorType.Name);
        }

        /// <summary>
        /// The actors type name.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>The type name without the 'Actor' part.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string ActorName(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.ArgumentIsNullEmptyOrWhiteSpace, nameof(typeName)));
            }
            return typeName.Replace(nameof(Actor), string.Empty, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Send a query and returns the result.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="query">The message.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">actor or message</exception>
        public static async Task<T> Ask<T>(this IDispatchActor actor, IQuery<T> query)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            return (T)await actor.Ask(new Envelope(query));
        }

        /// <summary>
        /// Sends a notification message.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">actor or message</exception>
        public static Task Notify(this IDispatchActor actor, IMessage message)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            return actor.Notify(new Envelope(message));
        }

        /// <summary>
        /// Sends a command and wait for the execution ends.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="command">The message.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">actor or message</exception>
        public static Task Tell(this IDispatchActor actor, ICommand command)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            return actor.Tell(new Envelope(command));
        }
    }
}
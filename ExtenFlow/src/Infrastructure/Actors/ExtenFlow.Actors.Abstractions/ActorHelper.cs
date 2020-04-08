using System;
using System.Globalization;
using System.Resources;

using Dapr.Actors.Runtime;

[assembly: NeutralResourcesLanguage("en")]

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
    }
}
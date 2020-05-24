using System;
using System.Globalization;

using ExtenFlow.Domain.Aggregates;

namespace ExtenFlow.Domain.Exceptions
{
    /// <summary>
    /// Class EntityStateNotInitializedException. Implements the <see cref="System.InvalidOperationException"/>
    /// </summary>
    /// <seealso cref="System.InvalidOperationException"/>
    public class EntityStateNotInitializedException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityStateNotInitializedException"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public EntityStateNotInitializedException(IEntity entity)
            : base(string.Format(CultureInfo.CurrentCulture, Properties.Resources.EntityStateNotInitialized, entity?.EntityName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityStateNotInitializedException"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">The entity property.</param>
        public EntityStateNotInitializedException(IEntity entity, string propertyName)
            : base(string.Format(CultureInfo.CurrentCulture, Properties.Resources.EntityPropertyStateNotInitialized, entity?.EntityName, propertyName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityStateNotInitializedException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EntityStateNotInitializedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityStateNotInitializedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        public EntityStateNotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityStateNotInitializedException"/> class.
        /// </summary>
        public EntityStateNotInitializedException()
        {
        }
    }
}
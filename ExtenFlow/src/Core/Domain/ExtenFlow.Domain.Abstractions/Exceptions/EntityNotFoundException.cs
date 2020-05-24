using System;
using System.Globalization;

using ExtenFlow.Domain.Aggregates;

namespace ExtenFlow.Domain.Exceptions
{
    /// <summary>
    /// Class EntityNotFoundException. Implements the <see cref="System.Exception"/>
    /// </summary>
    /// <seealso cref="System.Exception"/>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public EntityNotFoundException(IEntity entity)
            : base(string.Format(CultureInfo.CurrentCulture, Properties.Resources.EntityNotFound, entity?.EntityName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">The entity property.</param>
        public EntityNotFoundException(IEntity entity, string propertyName)
            : base(string.Format(CultureInfo.CurrentCulture, Properties.Resources.EntityPropertyStateNotInitialized, entity?.EntityName, propertyName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EntityNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        public EntityNotFoundException()
        {
        }
    }
}
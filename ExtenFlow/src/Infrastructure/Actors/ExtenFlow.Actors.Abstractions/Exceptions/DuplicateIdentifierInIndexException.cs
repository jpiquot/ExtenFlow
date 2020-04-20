using System;
using System.Globalization;

namespace ExtenFlow.Actors.Exceptions
{
    /// <summary>
    /// Class DuplicateIdentifierInIndexException. Implements the <see cref="System.Exception"/>
    /// </summary>
    /// <seealso cref="System.Exception"/>
    public class DuplicateIdentifierInIndexException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateIdentifierInIndexException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DuplicateIdentifierInIndexException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateIdentifierInIndexException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        public DuplicateIdentifierInIndexException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateIdentifierInIndexException"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="indexName"></param>
        /// <param name="identifier"></param>
        /// <param name="existingKey"></param>
        /// <param name="innerException">The inner exception.</param>
        /// <remarks>
        /// Message: Item with Id='{0}' already exist in unique index {1} with another Key='{2}'.
        /// </remarks>
        public DuplicateIdentifierInIndexException(CultureInfo culture, string indexName, string identifier, string existingKey, Exception? innerException = null)
            : base(string.Format(culture, Properties.Resources.DuplicateIndexIdentifier, identifier, indexName, existingKey), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateIdentifierInIndexException"/> class.
        /// </summary>
        public DuplicateIdentifierInIndexException()
        {
        }
    }
}
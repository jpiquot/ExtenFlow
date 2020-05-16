using System;
using System.Globalization;

namespace ExtenFlow.Actors.Exceptions
{
    /// <summary>
    /// Class DuplicateKeyInIndexException. Implements the <see cref="System.Exception"/>
    /// </summary>
    /// <seealso cref="System.Exception"/>
    public class DuplicateKeyInIndexException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyInIndexException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DuplicateKeyInIndexException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyInIndexException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        public DuplicateKeyInIndexException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyInIndexException"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="indexName"></param>
        /// <param name="key"></param>
        /// <param name="existingId"></param>
        /// <param name="innerException">The inner exception.</param>
        /// <remarks>
        /// Message: Item with Key='{0}' already exist in unique index {1} with another Id='{2}'.
        /// </remarks>
        public DuplicateKeyInIndexException(CultureInfo culture, string indexName, string key, string existingId, Exception? innerException = null)
            : base(string.Format(culture, Properties.Resources.DuplicateIndexIdentifier, key, indexName, existingId), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyInIndexException"/> class.
        /// </summary>
        public DuplicateKeyInIndexException()
        {
        }
    }
}
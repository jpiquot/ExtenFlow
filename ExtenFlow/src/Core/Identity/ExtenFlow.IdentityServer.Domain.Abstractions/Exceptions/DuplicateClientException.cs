using System;
using System.Globalization;

namespace ExtenFlow.IdentityServer.Domain.Exceptions
{
    /// <summary>
    /// Class DuplicateClientException. Implements the <see cref="System.Exception"/>
    /// </summary>
    /// <seealso cref="System.Exception"/>
    public class DuplicateClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateClientException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DuplicateClientException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateClientException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        public DuplicateClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateClientException"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="value">The value.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        /// <remarks>Message : Duplicate client with '{0}'='{1}'.</remarks>
        public DuplicateClientException(CultureInfo culture, string criteria, string? value, Exception? innerException = null)
            : base(string.Format(culture, Properties.Resources.DuplicateClient, criteria, value), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateClientException"/> class.
        /// </summary>
        public DuplicateClientException()
        {
        }
    }
}
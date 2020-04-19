using System;
using System.Globalization;

namespace ExtenFlow.Identity.Users.Exceptions
{
    /// <summary>
    /// Class DuplicateUserException. Implements the <see cref="System.Exception"/>
    /// </summary>
    /// <seealso cref="System.Exception"/>
    public class DuplicateUserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateUserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DuplicateUserException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateUserException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        public DuplicateUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateUserException"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="value">The value.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        /// <remarks>Message : Duplicate user with '{0}'='{1}'.</remarks>
        public DuplicateUserException(CultureInfo culture, string criteria, string? value, Exception? innerException = null)
            : base(string.Format(culture, Properties.Resources.DuplicateUser, criteria, value), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateUserException"/> class.
        /// </summary>
        public DuplicateUserException()
        {
        }
    }
}
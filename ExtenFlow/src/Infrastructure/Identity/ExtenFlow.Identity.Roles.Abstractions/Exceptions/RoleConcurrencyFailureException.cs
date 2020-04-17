using System;
using System.Globalization;

namespace ExtenFlow.Identity.Roles.Exceptions
{
    /// <summary>
    /// Class RoleConcurrencyFailureException. Implements the <see cref="System.Exception"/>
    /// </summary>
    /// <seealso cref="System.Exception"/>
    public class RoleConcurrencyFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleConcurrencyFailureException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RoleConcurrencyFailureException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleConcurrencyFailureException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        public RoleConcurrencyFailureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleConcurrencyFailureException"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="expected">Expected concurrency stamp.</param>
        /// <param name="actual">Actual concurrency stamp in the database.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <span
        /// class="keyword"><span class="languageSpecificText"><span class="cs">null</span><span
        /// class="vb">Nothing</span><span class="cpp">nullptr</span></span></span><span
        /// class="nu">a null reference ( <span class="keyword">Nothing</span> in Visual
        /// Basic)</span> in Visual Basic) if no inner exception is specified.
        /// </param>
        /// <remarks>
        /// Message : Concurrency check failure when updating the database. Expected stamp : '{0}';
        /// Actual stamp : '{1}'.
        /// </remarks>
        public RoleConcurrencyFailureException(CultureInfo culture, string? expected, string? actual, Exception? innerException = null)
            : base(string.Format(culture, Properties.Resources.RoleConcurrencyFailure, expected, actual), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleConcurrencyFailureException"/> class.
        /// </summary>
        public RoleConcurrencyFailureException()
        {
        }
    }
}
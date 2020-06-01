using System;
using System.Globalization;

namespace ExtenFlow.Identity.Roles.Domain.Exceptions
{
    /// <summary>
    /// Class RoleNotFoundException. Implements the <see cref="System.Exception"/>
    /// </summary>
    /// <seealso cref="System.Exception"/>
    public class RoleNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RoleNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        public RoleNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNotFoundException"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="value">The value.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        /// <remarks>Message : Role with '{0}'='{1}' not found.</remarks>
        public RoleNotFoundException(CultureInfo culture, string criteria, string? value, Exception? innerException = null)
            : base(string.Format(culture, Properties.Resources.RoleNotFound, criteria, value), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNotFoundException"/> class.
        /// </summary>
        public RoleNotFoundException()
        {
        }
    }
}
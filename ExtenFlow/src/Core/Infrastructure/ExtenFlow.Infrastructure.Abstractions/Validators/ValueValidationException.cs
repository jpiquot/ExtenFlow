using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtenFlow.Infrastructure
{
    /// <summary>
    /// Class ValueValidationException. Implements the <see cref="System.Exception"/>
    /// </summary>
    /// <seealso cref="System.Exception"/>
    public class ValueValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueValidationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ValueValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueValidationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        public ValueValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueValidationException"/> class.
        /// </summary>
        public ValueValidationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueValidationException"/> class.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public ValueValidationException(IList<ValidatorMessage> messages)
            : base(string.Join('\n', messages.Select(p => p.ToString()).ToList()))
        {
        }
    }
}
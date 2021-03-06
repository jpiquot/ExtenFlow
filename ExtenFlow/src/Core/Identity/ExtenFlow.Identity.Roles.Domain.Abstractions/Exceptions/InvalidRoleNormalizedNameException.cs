﻿using System;
using System.Globalization;

namespace ExtenFlow.Identity.Roles.Domain.Exceptions
{
    /// <summary>
    /// Class DuplicateRoleException. Implements the <see cref="System.Exception"/>
    /// </summary>
    /// <seealso cref="System.Exception"/>
    public class InvalidRoleNormalizedNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRoleNormalizedNameException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidRoleNormalizedNameException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRoleNormalizedNameException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        public InvalidRoleNormalizedNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRoleNormalizedNameException"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="value">The value.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference ( <see
        /// langword="Nothing"/> in Visual Basic) if no inner exception is specified.
        /// </param>
        /// <remarks>Message : Invalid role name : '{0}'.</remarks>
        public InvalidRoleNormalizedNameException(CultureInfo culture, string? value, Exception? innerException = null)
            : base(string.Format(culture, Properties.Resources.InvalidRoleNormalizedName, value), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRoleNormalizedNameException"/> class.
        /// </summary>
        public InvalidRoleNormalizedNameException()
        {
        }
    }
}
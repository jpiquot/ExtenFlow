using System;

namespace ExtenFlow.Infrastructure
{
    /// <summary>
    /// Enum ValidatorMessageLevel
    /// </summary>
    public enum ValidatorMessageLevel
    {
        /// <summary>
        /// The information
        /// </summary>
        Information = 0,

        /// <summary>
        /// The warning
        /// </summary>
        Warning = 1,

        /// <summary>
        /// The error
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// Class ValidatorMessage.
    /// </summary>
    public class ValidatorMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorMessage"/> class.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException">message</exception>
        public ValidatorMessage(ValidatorMessageLevel level, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
            Level = level;
            Message = message;
            DateTime = DateTimeOffset.Now;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public DateTimeOffset DateTime { get; }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        public ValidatorMessageLevel Level { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString() => $"[{Level}]\t({DateTime:O}) : \n{Message}";
    }
}
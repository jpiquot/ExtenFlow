#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Collections.Generic;

using ExtenFlow.Infrastructure;

namespace ExtenFlow.Messages
{
    /// <summary>
    /// Request validation
    /// </summary>
    public abstract class RequestValidator : MessageValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected RequestValidator(string? instanceName) : base(instanceName)
        {
        }

        /// <summary>
        /// Validates the message.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateMessage(IMessage value)
        {
            if (value is IRequest request)
            {
                return ValidateRequest(request);
            }
            return new[] { TypeMismatchError<IRequest>(value) };
        }

        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected virtual IList<ValidatorMessage> ValidateRequest(IRequest value) => new List<ValidatorMessage>();
    }
}
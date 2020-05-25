#pragma warning disable CA1710 // Identifiers should have correct suffix

using System.Collections.Generic;

using ExtenFlow.Infrastructure;

namespace ExtenFlow.Domain.Validators
{
    /// <summary>
    /// Command validation
    /// </summary>
    public abstract class CommandValidator : RequestValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected CommandValidator(string? instanceName) : base(instanceName)
        {
        }

        /// <summary>
        /// Validates the command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected virtual IList<ValidatorMessage> ValidateCommand(ICommand value) => new List<ValidatorMessage>();

        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRequest(IRequest value)
        {
            if (value is ICommand command)
            {
                return ValidateCommand(command);
            }
            return new[] { TypeMismatchError<ICommand>(value) };
        }
    }
}
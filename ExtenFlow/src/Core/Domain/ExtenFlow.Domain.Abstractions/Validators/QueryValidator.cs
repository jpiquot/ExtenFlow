using System.Collections.Generic;

using ExtenFlow.Infrastructure;

namespace ExtenFlow.Domain.Validators
{
    /// <summary>
    /// Query validation
    /// </summary>
    public abstract class QueryValidator : RequestValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected QueryValidator(string? instanceName) : base(instanceName)
        {
        }

        /// <summary>
        /// Validates the query.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected virtual IList<ValidatorMessage> ValidateQuery(IQuery value) => new List<ValidatorMessage>();

        /// <summary>
        /// Validates the request.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateRequest(IRequest value)
        {
            if (value is IQuery query)
            {
                return ValidateQuery(query);
            }
            return new[] { TypeMismatchError<IQuery>(value) };
        }
    }
}
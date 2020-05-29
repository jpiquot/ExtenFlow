using System.Collections.Generic;
using System.Globalization;

using ExtenFlow.Identity.Roles.ValueObjects;
using ExtenFlow.Infrastructure;
using ExtenFlow.Messages;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Queries
{
    /// <summary>
    /// Role query validation
    /// </summary>
    public abstract class RoleQueryValidator<T> : QueryValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleQueryValidator{T}"/> class.
        /// </summary>
        /// <param name="instanceName">The instance.</param>
        protected RoleQueryValidator(string? instanceName) : base(instanceName)
        {
        }

        /// <summary>
        /// Validates the query.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected override IList<ValidatorMessage> ValidateQuery(IQuery value)
        {
            var messages = new List<ValidatorMessage>();
            if (value is RoleQuery<T> query)
            {
                messages.AddRange(new RoleIdValidator(InstanceName, nameof(IMessage.AggregateId)).Validate(query.AggregateId));
                if (query.AggregateType != AggregateName.Role)
                {
                    messages.Add(new ValidatorMessage(
                        ValidatorMessageLevel.Error,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Domain.Properties.Resources.AggregateTypeMismatch,
                            AggregateName.Role,
                            query.AggregateType
                            )));
                }
                messages.AddRange(ValidateRoleQuery(query));
            }
            else
            {
                messages.Add(TypeMismatchError<RoleQuery<T>>(value));
            }
            return messages;
        }

        /// <summary>
        /// Validates the role query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected virtual IList<ValidatorMessage> ValidateRoleQuery(RoleQuery<T> query)
            => new List<ValidatorMessage>();
    }
}
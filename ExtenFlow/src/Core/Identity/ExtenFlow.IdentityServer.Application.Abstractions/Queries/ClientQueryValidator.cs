using System.Collections.Generic;
using System.Globalization;

using ExtenFlow.IdentityServer.Domain.ValueObjects;
using ExtenFlow.Infrastructure;
using ExtenFlow.Messages;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.IdentityServer.Application.Queries
{
    /// <summary>
    /// Role query validation
    /// </summary>
    public abstract class ClientQueryValidator<T> : QueryValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientQueryValidator{T}"/> class.
        /// </summary>
        /// <param name="instanceName">The instance.</param>
        protected ClientQueryValidator(string? instanceName) : base(instanceName)
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
            if (value is ClientQuery<T> query)
            {
                messages.AddRange(new ClientIdValidator(InstanceName, nameof(IMessage.AggregateId)).Validate(query.AggregateId));
                if (query.AggregateType != AggregateName.Role)
                {
                    messages.Add(new ValidatorMessage(
                        ValidatorMessageLevel.Error,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            ExtenFlow.Domain.Properties.Resources.AggregateTypeMismatch,
                            AggregateName.Role,
                            query.AggregateType
                            )));
                }
                messages.AddRange(ValidateRoleQuery(query));
            }
            else
            {
                messages.Add(TypeMismatchError<ClientQuery<T>>(value));
            }
            return messages;
        }

        /// <summary>
        /// Validates the role query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected virtual IList<ValidatorMessage> ValidateRoleQuery(ClientQuery<T> query)
            => new List<ValidatorMessage>();
    }
}
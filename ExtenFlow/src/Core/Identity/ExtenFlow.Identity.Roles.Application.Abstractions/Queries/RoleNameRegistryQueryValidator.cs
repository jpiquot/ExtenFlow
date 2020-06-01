using System.Collections.Generic;
using System.Globalization;

using ExtenFlow.Identity.Roles.Domain.ValueObjects;
using ExtenFlow.Infrastructure;
using ExtenFlow.Messages;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Application.Queries
{
    /// <summary>
    /// Role name registry query validation
    /// </summary>
    public abstract class RoleNameRegistryQueryValidator<T> : QueryValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleNameRegistryQueryValidator{T}"/> class.
        /// </summary>
        /// <param name="instanceName">The instance.</param>
        protected RoleNameRegistryQueryValidator(string? instanceName) : base(instanceName)
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
            if (value is RoleNameRegistryQuery<T> query)
            {
                messages.AddRange(new RoleNormalizedNameValidator(InstanceName, nameof(IMessage.AggregateId)).Validate(query.AggregateId));
                if (query.AggregateType != AggregateName.RoleNameRegistry)
                {
                    messages.Add(new ValidatorMessage(
                        ValidatorMessageLevel.Error,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            ExtenFlow.Domain.Properties.Resources.AggregateTypeMismatch,
                            AggregateName.RoleNameRegistry,
                            query.AggregateType
                            )));
                }
                messages.AddRange(ValidateRoleNameRegistryQuery(query));
            }
            else
            {
                messages.Add(TypeMismatchError<RoleNameRegistryQuery<T>>(value));
            }
            return messages;
        }

        /// <summary>
        /// Validates the role name registry query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>IList&lt;ValidatorMessage&gt;.</returns>
        protected virtual IList<ValidatorMessage> ValidateRoleNameRegistryQuery(RoleNameRegistryQuery<T> query)
            => new List<ValidatorMessage>();
    }
}
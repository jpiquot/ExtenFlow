using ExtenFlow.Identity.Roles.Commands;
using ExtenFlow.Domain.Validators;

using FluentValidation;

#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Identity.Roles.Validators
{
    /// <summary>
    /// Role command validation
    /// </summary>
    public class RoleCommandValidator<T> : CommandValidator<T> where T : RoleCommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RoleCommandValidator(bool aggregateIdRequired = true) : base(aggregateIdRequired)
        {
            RuleFor(command => command.AggregateType).Equal(RoleCommand.DefaultAggregateType);
        }
    }
}
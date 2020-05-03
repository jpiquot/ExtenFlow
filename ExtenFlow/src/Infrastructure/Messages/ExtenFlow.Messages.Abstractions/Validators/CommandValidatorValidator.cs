#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Messages.Validators
{
    /// <summary>
    /// Command validation
    /// </summary>
    public abstract class CommandValidator<T> : RequestValidator<T> where T : ICommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected CommandValidator(bool aggregateIdRequired = true) : base(aggregateIdRequired)
        {
        }
    }
}
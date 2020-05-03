#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Messages.Validators
{
    /// <summary>
    /// Event validation
    /// </summary>
    public abstract class EventValidator<T> : MessageValidator<T> where T : IEvent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected EventValidator(bool aggregateIdRequired = true) : base(aggregateIdRequired)
        {
        }
    }
}
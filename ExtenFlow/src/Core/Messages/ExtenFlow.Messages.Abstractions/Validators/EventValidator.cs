#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Messages
{
    /// <summary>
    /// Event validation
    /// </summary>
    public abstract class EventValidator : MessageValidator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected EventValidator(string? instanceName) : base(instanceName)
        {
        }
    }
}
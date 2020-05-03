#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Messages.Validators
{
    /// <summary>
    /// Request validation
    /// </summary>
    public abstract class RequestValidator<T> : MessageValidator<T> where T : IRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected RequestValidator(bool aggregateIdRequired = true) : base(aggregateIdRequired)
        {
        }
    }
}